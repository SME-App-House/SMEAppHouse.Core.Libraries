using Microsoft.EntityFrameworkCore;

namespace SMED.Core.Patterns.EF.StrategyForDBCtxt
{
    public interface IDbContextFactory<T> where T : DbContext
    {
        T CreateDbContext();
        T CreateDbContext(string connectionString);
    }
}