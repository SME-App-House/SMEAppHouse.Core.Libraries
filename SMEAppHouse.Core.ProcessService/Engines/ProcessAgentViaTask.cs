using System;
using System.Threading;
using System.Threading.Tasks;
using SMEAppHouse.Core.ProcessService.Engines.Interfaces;

namespace SMEAppHouse.Core.ProcessService.Engines
{
    public abstract class ProcessAgentViaTask : IProcessAgent
    {
        //private readonly Action m_loopedAction = null;
        private readonly AutoResetEvent _mPauseEvent;
        private readonly AutoResetEvent _mResumeEvent;
        private readonly AutoResetEvent _mStopEvent;
        private readonly AutoResetEvent _mWaitEvent;

        private readonly object _mLocker = new object();
        public EngineStatusEnum EngineStatus { get; set; }= EngineStatusEnum.NonState;
        public TimeSpan PauseTime { get; set; }
        public int PauseBetween { get; set; }

        public Action<bool> BeforeSuspendCallback { get; set; }
        public Action AfterSuspendCallback { get; set; }
        public Action<bool> BeforeResumeCallback { get; set; }
        public Action AfterResumeCallback { get; set; }
        public Action<bool> BeforeStopCallback { get; set; }
        public Action<bool> BeforeStartCallback { get; set; }
        public Action AfterStartCallback { get; set; }
        public Action AfterShutdownCallback { get; set; }
        public Action ProcessActionCallback { get; set; }

        protected ProcessAgentViaTask()
            : this(TimeSpan.FromSeconds(1))
        {
        }

        protected ProcessAgentViaTask(int milliSeconds)
            : this(TimeSpan.FromMilliseconds(milliSeconds))
        {
        }

        protected ProcessAgentViaTask(TimeSpan pauseTime)
        {
            PauseTime = pauseTime;

            _mPauseEvent = new AutoResetEvent(false);
            _mResumeEvent = new AutoResetEvent(false);
            _mStopEvent = new AutoResetEvent(false);
            _mWaitEvent = new AutoResetEvent(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeout"></param>
        public virtual void Suspend(int timeout = 0)
        {
            var _cancel = false;

            BeforeSuspendCallback?.Invoke(_cancel);

            if (_cancel) return;

            _mPauseEvent.Set();

            EngineStatus = EngineStatusEnum.Suspended;

            AfterSuspendCallback?.Invoke();

            _mWaitEvent.WaitOne(timeout);
        }

        public virtual void Shutdown(int timeout = 0)
        {
            bool _cancel = false;

            BeforeStopCallback?.Invoke(_cancel);

            if (!_cancel)
            {
                _mStopEvent.Set();
                _mResumeEvent.Set();

                EngineStatus = EngineStatusEnum.NonState;

                AfterShutdownCallback?.Invoke();
            }
        }

        public virtual void Start()
        {
            var cancel = false;

            BeforeStartCallback?.Invoke(cancel);

            if (cancel) return;

            Task.Factory
                .StartNew(ServiceActionLoop)
                .ContinueWith(p =>
                {
                    p.Exception?.Handle(x =>
                    {
                        Console.WriteLine(x.Message);
                        return false;
                    });
                });

            EngineStatus = EngineStatusEnum.Resumed;

            AfterStartCallback?.Invoke();
        }

        bool _firstTimeTick = true;
        private void ServiceActionLoop()
        {
            Action executePause = () =>
            {
                if (_mPauseEvent.WaitOne(PauseTime))
                {
                    _mWaitEvent.Set();
                    _mResumeEvent.WaitOne(Timeout.Infinite);
                }
                Thread.Sleep(5);
            };

            lock (_mLocker)
            {
                do
                {
                    if (_firstTimeTick)
                    {
                        _firstTimeTick = false;
                        executePause();
                        continue;
                    }

                    ServiceActionCallback();
                    ProcessActionCallback?.Invoke();
                    executePause();
                }
                while (!_mStopEvent.WaitOne(0));
            }
        }

        bool cancel = false;

        public virtual void Resume()
        {
            BeforeResumeCallback?.Invoke(cancel);

            if (cancel) return;

            _mResumeEvent.Set();

            EngineStatus = EngineStatusEnum.Resumed;

            AfterResumeCallback?.Invoke();
        }

        public void WaitForPause()
        {
            Suspend(Timeout.Infinite);
        }

        /// <summary>
        /// Returns the thread this object is on.
        /// </summary>
        public Thread Thread => System.Threading.Thread.CurrentThread;

        /// <summary>
        /// Enforce that this method is executed for ALL derived classes
        /// </summary>
        protected abstract void ServiceActionCallback();

    }
}
