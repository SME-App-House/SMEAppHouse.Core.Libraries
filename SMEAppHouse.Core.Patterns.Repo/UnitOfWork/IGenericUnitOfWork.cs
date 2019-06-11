using System;
using Microsoft.EntityFrameworkCore;
using SMEAppHouse.Core.Patterns.EF.ModelComposite;
using SMEAppHouse.Core.Patterns.Repo.Repository;

namespace SMEAppHouse.Core.Patterns.Repo.UnitOfWork
{
    public interface IGenericUnitOfWork<TPk> : IDisposable
        where TPk : struct
    {
        IRepository<TEntity, TPk> GetRepository<TEntity>()
            where TEntity : class, IGenericEntityBase<TPk>;

        int SaveChanges();
    }

    public interface IGenericUnitOfWork<out TContext, TPk> : IGenericUnitOfWork <TPk>
        where TContext : DbContext
        where TPk : struct
    {
        TContext DbContext { get; }
    }
}
