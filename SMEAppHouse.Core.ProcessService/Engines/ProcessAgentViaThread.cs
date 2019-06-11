using System;
using System.Threading;

namespace SMEAppHouse.Core.ProcessService.Engines
{
    public abstract class ProcessAgentViaThread : ProcessAgentBase
    {
        private Thread _thread = null;
        private readonly bool _isBackground = false;

        #region constructors

        protected ProcessAgentViaThread()
            : this(TimeSpan.FromSeconds(1))
        {
        }

        protected ProcessAgentViaThread(int pauseMilliSeconds, bool isBackground = true, bool autoInitialize = false, bool autoStart = false)
            : this(TimeSpan.FromMilliseconds(pauseMilliSeconds), isBackground, autoInitialize, autoStart)
        {
        }

        protected ProcessAgentViaThread(TimeSpan pauseTime, bool isBackground = true, bool autoInitialize = false, bool autoStart = false)
            : base(pauseTime, autoInitialize, autoStart)
        {
            _isBackground = isBackground;
        }

        #endregion

        #region the engine..

        internal override void ServiceActionInitialize()
        {
            if (_thread != null && _thread.IsAlive)
                return;

            _thread = new Thread(base.ServiceActionEngine)
            {
                IsBackground = _isBackground
            };
            _thread.Start();
        }

        internal override void ServiceActionOnShutdown(int timeOut)
        {
            if (_thread.IsAlive)
                _thread.Join(timeOut);

        }

        #endregion

    }
}
