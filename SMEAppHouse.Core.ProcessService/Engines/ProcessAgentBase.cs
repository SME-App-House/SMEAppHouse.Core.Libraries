using System;
using System.Threading;
using System.Threading.Tasks;
using SMEAppHouse.Core.ProcessService.Engines.Interfaces;

namespace SMEAppHouse.Core.ProcessService.Engines
{
    public abstract class ProcessAgentBase : IProcessAgent
    {
        private readonly AutoResetEvent _stopEvent;
        private AutoResetEvent _intervalDelayEvent;
        private AutoResetEvent _pauseEvent;

        private AgentCommandSlag _agentCmdSlag;
        
        private volatile bool _firstTimeTick = true;
        private volatile bool _initialized = false;
        private volatile bool _running = false;
        private readonly object _locker = new object();

        public EngineStatusEnum EngineStatus { get; set; } = EngineStatusEnum.NonState;

        public TimeSpan IntervalDelay { get; set; }
        public bool AutoActivate { get; set; }
        public bool AutoStart { get; set; }

        public event BeforeEngineStatusChangedEventHandler OnBeforeEngineChanged;
        public event EngineStatusChangedEventHandler OnEngineStatusChanged;
        public event EventHandler OnReadyState;

        #region constructors

        protected ProcessAgentBase()
            : this(TimeSpan.FromSeconds(1))
        {
        }

        protected ProcessAgentBase(int intervalDelay, bool autoActivate = false, bool autoStart = false)
            : this(TimeSpan.FromMilliseconds(intervalDelay), autoActivate, autoStart)
        {
        }

        protected ProcessAgentBase(TimeSpan intervalDelay, bool autoActivate = false, bool autoStart = false)
        {
            IntervalDelay = intervalDelay;

            _stopEvent = new AutoResetEvent(false);
            _intervalDelayEvent = new AutoResetEvent(true);
            _pauseEvent = new AutoResetEvent(false);

            AutoActivate = autoActivate;
            AutoStart = autoStart;

            Task.Delay(1000)
                .ContinueWith(t =>
                {
                    if (AutoActivate)
                        Activate();
                });
        }

        #endregion

        #region the engine..

        /// <summary>
        /// Enforce that this method is executed for ALL derived classes
        /// </summary>
        protected abstract void ServiceActionCallback();

        /// <summary>
        /// Enforce that this method is executed for ALL derived classes
        /// </summary>
        internal abstract void ServiceActionInitialize();

        /// <summary>
        /// Actions performed from the inheriting class where it may optionally return the
        /// timeout in milliseconds to hold delay before the actual shutdown.
        /// </summary>
        /// <returns></returns>
        internal abstract void ServiceActionOnShutdown(int timeOut);

        /// <summary>
        /// 
        /// </summary>
        protected void ServiceActionEngine()
        {
            void ExecutePause()
            {
                if (_intervalDelayEvent.WaitOne(IntervalDelay))
                {
                    if (EngineStatus == EngineStatusEnum.NonState)
                    {
                        OnReadyState?.Invoke(this, new EventArgs());
                        _pauseEvent.WaitOne(Timeout.Infinite);

                        EngineStatus = EngineStatusEnum.RunningState;
                        OnEngineStatusChanged?.Invoke(this, new EngineStatusChangedEventArgs(EngineStatus));
                    }

                    if (_agentCmdSlag != null)
                    {
                        switch (_agentCmdSlag.StatusRequest)
                        {
                            case EngineStatusEnum.PausedState:
                                EngineStatus = EngineStatusEnum.PausedState;
                                OnEngineStatusChanged?.Invoke(this, new EngineStatusChangedEventArgs(EngineStatus));

                                _pauseEvent.WaitOne(
                                    _agentCmdSlag.TimeOut < 0 ? Timeout.Infinite : _agentCmdSlag.TimeOut);

                                EngineStatus = EngineStatusEnum.RunningState;
                                break;

                            case EngineStatusEnum.NonState:
                                _initialized = false;
                                _firstTimeTick = false;

                                _stopEvent.Set();
                                _intervalDelayEvent = new AutoResetEvent(true);
                                _pauseEvent= new AutoResetEvent(false);

                                ServiceActionOnShutdown(_agentCmdSlag.TimeOut);
                                EngineStatus = EngineStatusEnum.NonState;
                                
                                break;
                        }

                        _agentCmdSlag = null;
                        OnEngineStatusChanged?.Invoke(this, new EngineStatusChangedEventArgs(EngineStatus));
                    }
                    Thread.Sleep(1);
                }
            }

            lock (_locker)
            {
                do
                {
                    if (_firstTimeTick)
                    {
                        _firstTimeTick = false;
                        ExecutePause();
                        continue;
                    }

                    _running = true;
                    ServiceActionCallback();
                    ExecutePause();
                }
                while (!_stopEvent.WaitOne(0));
                // <- If millisecondsTimeout is zero, the method does not block.
                // It will test the state of the wait handle and returns immediately.
                _running = false;
            }
        }
        #endregion

        #region public methods

        /// <summary>
        /// 
        /// </summary>
        public Task Activate()
        {
            if (_initialized)
                return Task.CompletedTask;

            _initialized = true;
            return Task.Delay(1000)
                .ContinueWith(t =>
                {
                    _stopEvent.Reset();
                    ServiceActionInitialize();
                })
                .ContinueWith(t =>
                {
                    if (AutoStart)
                    {
                        _intervalDelayEvent.Set();
                        _pauseEvent.Set();
                    }
                });
        }

        /// <summary>
        /// 
        /// </summary>
        public void Resume()
        {
            if ((EngineStatus == EngineStatusEnum.RunningState && _running)
                | ShouldCancelAction(EngineStatusEnum.RunningState))
                return;

            if (!_initialized)
            {
                if (!AutoActivate)
                    throw new Exception("Activate required");
                else Activate();
            }
            else _pauseEvent.Set();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeout"></param>
        public void Suspend(TimeSpan timeout)
        {
            var millSecs = (int)timeout.TotalMilliseconds;
            Suspend(millSecs);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeoutMilliSecs"></param>
        public void Suspend(int timeoutMilliSecs = 0)
        {
            if (EngineStatus == EngineStatusEnum.NonState // not started yet
                | EngineStatus == EngineStatusEnum.PausedState // already suspended
                | ShouldCancelAction(EngineStatusEnum.PausedState)) // or client has cancelled.
                return;

            _agentCmdSlag = new AgentCommandSlag()
            {
                StatusRequest = EngineStatusEnum.PausedState,
                TimeOut = timeoutMilliSecs == 0 ? -1 : timeoutMilliSecs
            };
            _intervalDelayEvent.Set();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeOut"></param>
        public void Shutdown(int timeOut = 0)
        {
            if (EngineStatus == EngineStatusEnum.NonState
                | ShouldCancelAction(EngineStatusEnum.NonState))
                return;

            _agentCmdSlag = new AgentCommandSlag()
            {
                StatusRequest = EngineStatusEnum.NonState,
                TimeOut = timeOut
            };

            _intervalDelayEvent.Set();
            _pauseEvent.Set();
        }

        #endregion

        #region Private members

        internal bool ShouldCancelAction(EngineStatusEnum engineStatus)
        {
            var arg = new BeforeEngineStatusChangedEventArgs(engineStatus);
            OnBeforeEngineChanged?.Invoke(this, arg);
            return arg.Cancel;
        }

        internal class AgentCommandSlag
        {
            public EngineStatusEnum StatusRequest { get; set; }
            public int TimeOut { get; set; }
        }

        #endregion

    }
}
