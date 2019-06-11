using System;
using Microsoft.EntityFrameworkCore;

namespace SMEAppHouse.Core.Patterns.Repo.UnitOfWork.GuidPKBasedVariation
{
    public class UnitOfWork<TDbContext> : UnitOfWorkBase<TDbContext, Guid>, IUnitOfWork
        where TDbContext : DbContext, IDisposable
    {
        public UnitOfWork(TDbContext dbContext) : base(dbContext)
        {
        }
    }
}
