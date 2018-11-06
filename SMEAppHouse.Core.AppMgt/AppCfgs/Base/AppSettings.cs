
using SMEAppHouse.Core.AppMgt.AppCfgs.Interfaces;

namespace SMEAppHouse.Core.AppMgt.AppCfgs.Base
{
    public abstract class AppConfig : IAppConfig
    {
        //public string Title { get; set; }
        //public string Description { get; set; }
        public bool InDemoMode { get; set; }

        public AppEFBehaviorAttributes AppEFBehaviorAttributes { get; set; }

        public abstract void Validate();
    }
}