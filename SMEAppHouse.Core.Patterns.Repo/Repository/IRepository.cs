using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SMEAppHouse.Core.Patterns.EF.ModelComposite;

namespace SMEAppHouse.Core.Patterns.Repo.Repository
{
    public interface IRepository<TEntity, TPk> : IDisposable
        where TEntity : class, IGenericEntityBase<TPk>
        where TPk : struct
    {
        DbContext DbContext { get; set; }
        DbSet<TEntity> DbSet { get; set; }

        IQueryable<TEntity> Query(string sql, params object[] parameters);

        Task<TEntity> Search(params object[] keyValues);
        Task<TEntity> Search(Expression<Func<TEntity, object>> includeSelector, params object[] keyValues);

        //Task<TEntity> Search(CancellationToken cancelToken
        //                                    , params object[] keyValues);
        //Task<TEntity> Search(CancellationToken cancelToken
        //                                    , bool noTracking = true
        //                                    , params object[] keyValues);

        TEntity Single(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disableTracking = true);

        IEnumerable<TEntity> GetList(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int fetchLimit = 0, bool disableTracking = true);

        void Add(TEntity entity);
        void Add(params TEntity[] entities);
        void Add(IEnumerable<TEntity> entities);


        void Delete(TEntity entity);
        void Delete(object id);
        void Delete(params TEntity[] entities);
        void Delete(IEnumerable<TEntity> entities);


        void Update(TEntity entity);
        void Update(params TEntity[] entities);
        void Update(IEnumerable<TEntity> entities);
    }
}
