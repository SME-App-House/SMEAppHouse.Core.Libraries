namespace SMEAppHouse.Core.Patterns.Repo.Generic
{
    public interface IRepositoryReadOnly<T> : IReadRepository<T> where T : class
    {
       
    }
}