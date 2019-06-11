using System;
using Microsoft.EntityFrameworkCore;
using SMEAppHouse.Core.Patterns.EF.ModelComposite;

namespace SMEAppHouse.Core.Patterns.Repo.Repository.GuidPKBasedVariation
{
    public class Repository<TEntity> : RepositoryBase<TEntity, Guid>
        where TEntity : class, IGenericEntityBase<Guid>
    {
        public Repository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}