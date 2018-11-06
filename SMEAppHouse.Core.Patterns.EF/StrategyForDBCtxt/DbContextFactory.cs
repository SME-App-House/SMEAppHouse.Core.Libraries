using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace SMED.Core.Patterns.EF.StrategyForDBCtxt
{
    public class DbContextFactory<T> : IDbContextFactory<T>
        where T : DbContext, new()
    {
        private readonly IAppConfig _appConfig;
        private readonly IConfiguration _configuration;

        public DbContextFactory (IConfiguration configuration, IAppConfig appConfig)
        {
            _appConfig = appConfig;
            _configuration = configuration;
        }

        public T CreateDbContext()
        {
            return CreateDbContext(_configuration.GetConnectionString("AppDbConnection"));
        }

        public T CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<T>();
            optionsBuilder.UseSqlServer(connectionString, x =>
            {
                x.MigrationsHistoryTable(_appConfig.AppEFBehaviorAttributes.MigrationTblName, _appConfig.AppEFBehaviorAttributes.DbSchema);
            });
            var dbCntxOpt = optionsBuilder.Options;
            return (T)new DbContext(dbCntxOpt);
        }

    }
}
