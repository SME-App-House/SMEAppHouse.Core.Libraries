using Microsoft.EntityFrameworkCore;
using SMEAppHouse.Core.Patterns.EF.ModelComposite;

namespace SMEAppHouse.Core.Patterns.Repo.Repository.IntPKBasedVariation
{
    public class Repository<TEntity> : RepositoryBase<TEntity, int>
        where TEntity : class, IGenericEntityBase<int>
    {
        public Repository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}