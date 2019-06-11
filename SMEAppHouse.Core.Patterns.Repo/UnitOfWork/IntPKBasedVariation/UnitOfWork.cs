using System;
using Microsoft.EntityFrameworkCore;

namespace SMEAppHouse.Core.Patterns.Repo.UnitOfWork.IntPKBasedVariation
{
    public class UnitOfWork<TDbContext> : UnitOfWorkBase<TDbContext, int>, IUnitOfWork
    where TDbContext : DbContext, IDisposable
    {
        public UnitOfWork(TDbContext dbContext) : base(dbContext)
        {
        }
    }
}
