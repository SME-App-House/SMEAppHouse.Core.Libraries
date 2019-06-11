using Microsoft.EntityFrameworkCore;
using SMEAppHouse.Core.Patterns.EF.ModelComposite;

namespace SMEAppHouse.Core.Patterns.Repo.Repository.LongPKBasedVariation
{
    public class Repository<TEntity> : RepositoryBase<TEntity, long>
        where TEntity : class, IGenericEntityBase<long>
    {
        public Repository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}