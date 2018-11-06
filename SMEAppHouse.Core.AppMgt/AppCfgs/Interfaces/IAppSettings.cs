using SMEAppHouse.Core.AppMgt.AppCfgs.Base;
using SMEAppHouse.Core.AppMgt.AppCfgs.Validator;

namespace SMEAppHouse.Core.AppMgt.AppCfgs.Interfaces
{
    public interface IAppConfig : IValidatable
    {
        //string Title { get; set; }
        //string Description { get; set; }
        bool InDemoMode { get; set; }

        AppEFBehaviorAttributes AppEFBehaviorAttributes { get; set; }
        
    }

}
