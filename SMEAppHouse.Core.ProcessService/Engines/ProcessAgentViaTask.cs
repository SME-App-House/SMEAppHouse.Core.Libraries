using System;
using System.Threading.Tasks;

namespace SMEAppHouse.Core.ProcessService.Engines
{
    public abstract class ProcessAgentViaTask : ProcessAgentBase
    {

        #region constructors

        protected ProcessAgentViaTask()
            : this(TimeSpan.FromSeconds(1))
        {
        }

        protected ProcessAgentViaTask(int pauseMilliSeconds, bool autoInitialize = false, bool autoStart = false)
            : this(TimeSpan.FromMilliseconds(pauseMilliSeconds), autoInitialize, autoStart)
        {
        }

        protected ProcessAgentViaTask(TimeSpan pauseTime, bool autoInitialize = false, bool autoStart = false)
            : base(pauseTime, autoInitialize, autoStart)
        {
        }

        #endregion

        #region the engine..


        /// <summary>
        /// 
        /// </summary>
        internal override void ServiceActionInitialize()
        {
            Task.Factory
                .StartNew(base.ServiceActionEngine)
                .ContinueWith(p =>
                {
                    p.Exception?.Handle(x =>
                    {
                        Console.WriteLine(x.Message);
                        return false;
                    });
                });
        }

        internal override void ServiceActionOnShutdown(int timeOut)
        {
            //throw new NotImplementedException();
        }

        #endregion

    }
}
