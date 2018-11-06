using System;
using SMEAppHouse.Core.Patterns.EF.ModelComposite;
using SMEAppHouse.Core.Patterns.Repo.Base;

namespace SMEAppHouse.Core.Patterns.Repo.UnitOfWork
{
    /// <summary>
    /// Followed after: http://gaui.is/how-to-mock-the-datacontext-linq/
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        IRepositorySync<TEntity, TPk> GetRepository<TEntity, TPk>()
            where TEntity : class, IIdentifiableEntity<TPk>
            where TPk : struct;

        void Save();
    }
}
