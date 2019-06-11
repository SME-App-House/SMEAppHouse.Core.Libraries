using System;
using System.Threading;
using NLog;
using SMEAppHouse.Core.CodeKits.Tools;
using SMEAppHouse.Core.TopshelfAdapter.Common;

namespace SMEAppHouse.Core.TopshelfAdapter
{
    public abstract class TopshelfSocketBase<T> : ITopshelfClientExt where T : class
    {
        #region private variables to keep values

        private AppDotTicker<T> _ticker;

        private readonly object _mutex = new object();
        private readonly bool _lazyInitialization = false;
        private readonly AutoResetEvent _pauseEvent = new AutoResetEvent(false);
        private readonly AutoResetEvent _resumeEvent = new AutoResetEvent(false);
        private readonly AutoResetEvent _stopEvent = new AutoResetEvent(false);
        private readonly AutoResetEvent _waitEvent = new AutoResetEvent(false);

        private readonly int _milliSecsDelay;
        private readonly bool _isBackground;

        #endregion

        #region properties

        public InitializationStatusEnum InitializationStatus { get; set; }
        public bool IsPaused { get; private set; }
        public bool IsResumed { get; private set; }
        public bool IsTerminated { get; private set; }
        public Logger Logger { get; set; }

        /// <summary>
        /// Reference to the actual thread this object is using.
        /// </summary>
        public Thread ServiceThread => new Thread(ServiceLoop)
        {
            IsBackground = _isBackground
        };

        public event ServiceInitializedEventHandler OnServiceInitialized;

        #endregion

        #region constructors

        protected TopshelfSocketBase()
            : this(1)
        {
        }

        protected TopshelfSocketBase(TimeSpan pauseDelay)
            : this((int)pauseDelay.TotalMilliseconds, null)
        {
        }

        protected TopshelfSocketBase(TimeSpan pauseDelay, Logger logger)
            : this((int)pauseDelay.TotalMilliseconds, logger, false)
        {
        }

        protected TopshelfSocketBase(TimeSpan pauseDelay, Logger logger, bool isBackground)
            : this((int)pauseDelay.TotalMilliseconds, logger, isBackground, false)
        {
        }

        protected TopshelfSocketBase(TimeSpan pauseDelay, Logger logger, bool isBackground, bool lazyInitialization)
            : this((int)pauseDelay.TotalMilliseconds, logger, isBackground, lazyInitialization)
        {
        }

        protected TopshelfSocketBase(int milliSecsDelay)
            : this(1, null)
        {
        }

        protected TopshelfSocketBase(int milliSecsDelay, Logger logger)
            : this(milliSecsDelay, logger, false)
        {
        }

        protected TopshelfSocketBase(int milliSecsDelay, Logger logger, bool isBackground)
            : this(milliSecsDelay, logger, isBackground, false)
        {
        }

        protected TopshelfSocketBase(int milliSecsDelay, Logger logger, bool isBackground, bool lazyInitialization)
        {
            Logger = logger;
            _milliSecsDelay = milliSecsDelay;
            _isBackground = isBackground;
            _lazyInitialization = lazyInitialization;

            InitializeConsoleTicker();

            if (!_lazyInitialization)
                TryInitialize();

            ServiceThread.Start();
        }

        #endregion

        #region public methods

        public void Resume()
        {
            new Thread(() =>
            {
                if (_lazyInitialization)
                    TryInitialize();

                _resumeEvent.Set();
                _ticker.Resume();

                IsPaused = false;
                IsResumed = true;
                IsTerminated = false;

            })
            { IsBackground = true }
          .Start();
        }

        public void Suspend()
        {
            _pauseEvent.Set();
            _waitEvent.WaitOne(0);
            _ticker.Stop();

            IsPaused = true;
            IsResumed = false;
            IsTerminated = false;
        }

        public void Shutdown()
        {
            TryDestroy();
            _ticker.Shutdown();

            IsPaused = false;
            IsResumed = false;
            IsTerminated = true;

        }

        public void NLog(string data)
        {
            NLog(data, false);
        }

        public void NLog(string data, bool includeWriteToConsole)
        {
            NLog(NLogLevelEnum.Info, data, includeWriteToConsole);
        }

        public void NLog(NLogLevelEnum nLogLevel, string data)
        {
            NLog(nLogLevel, data, false);
        }

        public void NLog(NLogLevelEnum nLogLevel, string data, bool includeWriteToConsole)
        {
            switch (nLogLevel)
            {
                case NLogLevelEnum.Fatal:
                    Logger?.Fatal(data);
                    break;
                case NLogLevelEnum.Error:
                    Logger?.Error(data);
                    break;
                case NLogLevelEnum.Warn:
                    Logger?.Warn(data);
                    break;
                case NLogLevelEnum.Info:
                    Logger?.Info(data);
                    break;
                case NLogLevelEnum.Debug:
                    Logger?.Debug(data);
                    break;
                case NLogLevelEnum.Trace:
                    Logger?.Trace(data);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(nLogLevel), nLogLevel, null);
            }

            if (includeWriteToConsole) Console.WriteLine(data);
        }

        #endregion

        #region private methods

        /// <summary>
        /// 
        /// </summary>
        private void TryInitialize()
        {
            if (InitializationStatus == InitializationStatusEnum.Initialized | InitializationStatus == InitializationStatusEnum.Initializing) return;

            InitializationStatus = InitializationStatusEnum.Initializing;
            ServiceInitializeCallback();
            InitializationStatus = InitializationStatusEnum.Initialized;

            (new ServiceInitializedEventArgs()).InvokeEvent(this, OnServiceInitialized);
        }

        /// <summary>
        /// 
        /// </summary>
        private void TryDestroy()
        {
            if (InitializationStatus != InitializationStatusEnum.Initialized)
                return;
            ServiceTerminateCallback();
            InitializationStatus = InitializationStatusEnum.NonState;
        }

        /// <summary>
        /// Our fancy ticking dots
        /// </summary>
        private void InitializeConsoleTicker()
        {
            var ctr = 0;
            _ticker = new AppDotTicker<T>(100)
            {
                OnCompletionEvent = () => Console.WriteLine(@"Press ENTER to exit!"),
                OnTickEvent = () =>
                {
                    ctr++;
                    switch (ctr % 4)
                    {
                        case 0:
                            Console.Write("\r/");
                            ctr = 0;
                            break;
                        case 1:
                            Console.Write("\r-");
                            break;
                        case 2:
                            Console.Write("\r\\");
                            break;
                        case 3:
                            Console.Write("\r|");
                            break;
                    }

                    //Console.Write(@"."
                }
            };
        }

        protected abstract void ServiceInitializeCallback();
        protected abstract void ServiceTerminateCallback();
        protected abstract void ServiceActionCallback();

        /// <summary>
        /// 
        /// </summary>
        private void ServiceLoop()
        {
            lock (_mutex)
            {
                Thread.Sleep(5000);
                do
                {
                    if (InitializationStatus != InitializationStatusEnum.Initialized)
                        continue;

                    ServiceActionCallback();

                    if (_pauseEvent.WaitOne(_milliSecsDelay))
                    {
                        _waitEvent.Set();
                        _resumeEvent.WaitOne(Timeout.Infinite);
                    }
                    Thread.Sleep(1);
                } while (!_stopEvent.WaitOne(0));
            }
        }

        #endregion
    }
}
