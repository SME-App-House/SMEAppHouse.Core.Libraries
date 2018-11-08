using System;
using System.Threading;
using SMEAppHouse.Core.ProcessService.Engines.Interfaces;

namespace SMEAppHouse.Core.ProcessService.Engines
{
    public abstract class ProcessAgentViaThread : IProcessAgent
    {
        //private readonly Action m_loopedAction = null;
        private readonly AutoResetEvent _mPauseEvent;
        private readonly AutoResetEvent _mResumeEvent;
        private readonly AutoResetEvent _mStopEvent;
        private readonly AutoResetEvent _mWaitEvent;

        private readonly object _mLocker = new object();
        private readonly bool _isBackground = false;
        private bool _firstTimeTick = true;

        public EngineStatusEnum EngineStatus { get; set; }
        public TimeSpan PauseTime { get; set; }

        public int PauseBetween { get; set; }

        public Action<bool> BeforeStartCallback { get; set; }
        public Action AfterStartCallback { get; set; }
        public Action<bool> BeforeSuspendCallback { get; set; }
        public Action AfterSuspendCallback { get; set; }
        public Action<bool> BeforeResumeCallback { get; set; }
        public Action AfterResumeCallback { get; set; }
        public Action<bool> BeforeStopCallback { get; set; }
        public Action AfterShutdownCallback { get; set; }
        public Action ProcessActionCallback { get; set; }

        /// <summary>
        /// Reference to the actual thread this object is using.
        /// </summary>
        public Thread Thread => new Thread(Loop)
        {
            IsBackground = _isBackground
        };

        protected ProcessAgentViaThread(int pauseBetween = 1000, bool isBackground = false)
        {
            PauseBetween = pauseBetween;
            _isBackground = isBackground;

            _mPauseEvent = new AutoResetEvent(false);
            _mResumeEvent = new AutoResetEvent(false);
            _mStopEvent = new AutoResetEvent(false);
            _mWaitEvent = new AutoResetEvent(false);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Start()
        {
            var cancel = false;

            BeforeStartCallback?.Invoke(cancel);

            if (cancel) return;
            Thread.Start();

            EngineStatus = EngineStatusEnum.Resumed;

            if (AfterStartCallback != null)
                AfterStartCallback();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeout"></param>
        public virtual void Suspend(int timeout = 0)
        {
            var cancel = false;
            BeforeSuspendCallback?.Invoke(cancel);

            if (cancel)
                return;

            _mPauseEvent.Set();

            EngineStatus = EngineStatusEnum.Suspended;

            AfterSuspendCallback?.Invoke();

            _mWaitEvent.WaitOne(timeout);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Resume()
        {
            var _cancel = false;
            BeforeResumeCallback?.Invoke(_cancel);

            if (_cancel) return;

            if (!Thread.IsAlive)
                Thread.Start();

            _mResumeEvent.Set();

            EngineStatus = EngineStatusEnum.Resumed;

            AfterResumeCallback?.Invoke();
        }

        public virtual void Shutdown(int timeout = 0)
        {
            var cancel = false;

            BeforeStopCallback?.Invoke(cancel);

            if (cancel) return;

            _mStopEvent.Set();
            _mResumeEvent.Set();

            EngineStatus = EngineStatusEnum.NonState;

            if (Thread.IsAlive)
                Thread.Join(timeout);

            AfterShutdownCallback?.Invoke();
        }

        public void WaitForPause()
        {
            Suspend(Timeout.Infinite);
        }

        public void WaitForStop()
        {
            Shutdown(Timeout.Infinite);
        }

        /// <summary>
        /// 
        /// </summary>
        private void Loop()
        {
            Action executePause = () =>
            {
                if (_mPauseEvent.WaitOne(PauseBetween))
                {
                    _mWaitEvent.Set();
                    _mResumeEvent.WaitOne(Timeout.Infinite);
                }
                Thread.Sleep(10);
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

        /// <summary>
        /// 
        /// </summary>
        protected abstract void ServiceActionCallback();


    }
}
