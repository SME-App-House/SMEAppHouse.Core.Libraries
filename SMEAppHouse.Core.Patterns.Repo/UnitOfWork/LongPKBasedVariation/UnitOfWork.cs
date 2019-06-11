using System;
using Microsoft.EntityFrameworkCore;

namespace SMEAppHouse.Core.Patterns.Repo.UnitOfWork.LongPKBasedVariation
{
    public class UnitOfWork<TDbContext> : UnitOfWorkBase<TDbContext, long>, IUnitOfWork
        where TDbContext : DbContext, IDisposable
    {
        public UnitOfWork(TDbContext dbContext) : base(dbContext)
        {
        }
    }
}
