namespace SMEAppHouse.Core.AppMgt.AppCfgs.Interfaces
{
    public interface IAppEFBehaviorAttributes 
    {
        string MigrationTblName { get; set; }
        string DbSchema { get; set; }
    }
}
