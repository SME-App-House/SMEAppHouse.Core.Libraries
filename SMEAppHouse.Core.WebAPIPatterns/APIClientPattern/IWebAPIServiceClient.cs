using SMED.Core.Patterns.Entities;
using System.Collections.Generic;
using System.Net.Http;

namespace SMED.Core.WebAPI.Patterns.APIClientPattern
{
    public interface IWebAPIServiceClient<TEntity, in TPk>
        where TEntity : class, IEntity
    {
        HttpClient HttpClient { get; }
        string BaseServiceAddress { get;  }

        TEntity Create(TEntity entity);
        TEntity Update(TEntity entity);

        void RemoveById(TPk id);
        void RemoveAll();

        int Count();

        TEntity GetById(TPk id);

        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetAllWithEntities(params string[] entities);

    }
}