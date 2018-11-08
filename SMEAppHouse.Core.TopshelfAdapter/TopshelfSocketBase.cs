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

        private readonly int _pauseBetween;
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

        protected TopshelfSocketBase() :
            this(null)
        {
        }

        protected TopshelfSocketBase(Logger logger) : this(logger, 1000, false)
        {
        }

        protected TopshelfSocketBase(Logger logger, TimeSpan pauseBetween, bool isBackground = false) :
            this(logger, pauseBetween.Seconds, isBackground)
        {
        }

        protected TopshelfSocketBase(Logger logger, TimeSpan pauseBetween, bool isBackground = false, bool lazyInitialization = false) :
            this(logger, pauseBetween.Seconds, isBackground, lazyInitialization)
        {
        }

        protected TopshelfSocketBase(Logger logger, int pauseBetween = 1000, bool isBackground = false, bool lazyInitialization = false)
        {
            try
            {
                Logger = logger;
                _pauseBetween = pauseBetween;
                _isBackground = isBackground;
                _lazyInitialization = lazyInitialization;

                InitializeConsoleTicker();

                if (!_lazyInitialization)
                    TryInitialize();

                ServiceThread.Start();
            }
            catch (Exception exception)
            {
                throw;
            }

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
            NLog(data, true);
        }

        public void NLog(string data, bool includeWriteToConsole)
        {
            NLog(NLogLevelEnum.Info, data, includeWriteToConsole);
        }

        public void NLog(NLogLevelEnum nLogLevel, string data)
        {
            NLog(nLogLevel, data, true);
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
            try
            {
                if (InitializationStatus == InitializationStatusEnum.Initialized | InitializationStatus == InitializationStatusEnum.Initializing) return;

                InitializationStatus = InitializationStatusEnum.Initializing;
                ServiceInitializeCallback();
                InitializationStatus = InitializationStatusEnum.Initialized;

                (new ServiceInitializedEventArgs()).InvokeEvent(this, OnServiceInitialized);
            }
            catch (Exception exception)
            {
                throw;
            }
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
                        if (ctr % 4 == 0)
                        {
                            Console.Write("\r/");
                            ctr = 0;
                        }
                        else if (ctr % 4 == 1)
                        {
                            Console.Write("\r-");
                        }
                        else if (ctr % 4 == 2)
                        {
                            Console.Write("\r\\");
                        }
                        else if (ctr % 4 == 3)
                        {
                            Console.Write("\r|");
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
                do
                {
                    try
                    {
                        if (InitializationStatus != InitializationStatusEnum.Initialized)
                            continue;

                        ServiceActionCallback();

                        if (_pauseEvent.WaitOne(_pauseBetween))
                        {
                            _waitEvent.Set();
                            _resumeEvent.WaitOne(Timeout.Infinite);
                        }
                        Thread.Sleep(10);
                    }
                    catch (Exception exception)
                    {
                        throw;
                    }
                } while (!_stopEvent.WaitOne(0));
            }
        }

        #endregion
    }
}
