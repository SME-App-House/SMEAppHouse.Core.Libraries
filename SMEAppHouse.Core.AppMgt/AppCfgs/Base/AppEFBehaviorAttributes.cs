using SMEAppHouse.Core.AppMgt.AppCfgs.Interfaces;

namespace SMEAppHouse.Core.AppMgt.AppCfgs.Base
{
    public class AppEFBehaviorAttributes : IAppEFBehaviorAttributes
    {
        public string MigrationTblName { get; set; }
        public string DbSchema { get; set; } = "dbo";
    }
}
