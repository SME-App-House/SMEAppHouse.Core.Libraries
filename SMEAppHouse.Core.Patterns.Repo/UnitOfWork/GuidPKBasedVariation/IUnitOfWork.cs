using System;

namespace SMEAppHouse.Core.Patterns.Repo.UnitOfWork.GuidPKBasedVariation
{
    public interface IUnitOfWork : IGenericUnitOfWork<Guid>
    {
    }
}
