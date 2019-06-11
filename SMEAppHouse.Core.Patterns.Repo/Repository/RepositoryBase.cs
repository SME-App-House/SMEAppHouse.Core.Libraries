using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SMEAppHouse.Core.CodeKits.Exceptions;
using SMEAppHouse.Core.CodeKits.Tools;
using SMEAppHouse.Core.Patterns.EF.ModelComposite;

namespace SMEAppHouse.Core.Patterns.Repo.Repository
{
    public class RepositoryBase<TEntity, TPk> : IRepository<TEntity, TPk>
        where TEntity : class, IGenericEntityBase<TPk>
        where TPk : struct
    {
        public DbContext DbContext { get; set; }
        public DbSet<TEntity> DbSet { get; set; }

        public RepositoryBase(DbContext dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentException(nameof(dbContext));
            DbSet = DbContext.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> Query(string sql, params object[] parameters) => DbSet.FromSql(sql, parameters);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        public async Task<TEntity> Search(params object[] keyValues) => await DbSet.FindAsync(keyValues);
        public async Task<TEntity> Search(Expression<Func<TEntity, object>> includeSelector,
                                                            params object[] keyValues)
        {
            return await DbSet
                .Include(includeSelector )
                .FirstOrDefaultAsync(p => keyValues.Contains(p.Id));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="include"></param>
        /// <param name="disableTracking"></param>
        /// <returns></returns>
        public TEntity Single(Expression<Func<TEntity, bool>> predicate = null
            , Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null
            , bool disableTracking = true)
        {
            IQueryable<TEntity> query = DbSet;
            if (disableTracking) query = query.AsNoTracking();
            if (include != null) query = include(query);
            if (predicate != null) query = query.Where(predicate);
            return query.FirstOrDefault();
        }

        public IEnumerable<TEntity> GetList(
                    Expression<Func<TEntity, bool>> filter = null,
                    Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                    int fetchLimit = 0, bool disableTracking = true)
        {
            try
            {
                var result = RetryCodeKit.Do(() =>
                {
                    IQueryable<TEntity> query = DbSet;

                    if (disableTracking) query = query.AsNoTracking();
                    if (filter != null) query = query.Where(filter);
                    if (include != null) query = include(query);

                    //if (!string.IsNullOrEmpty(includeProperties))
                    //    query = includeProperties
                    //        .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    //        .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

                    if (fetchLimit > 0)
                    {
                        var resultsWithLimit = (orderBy?.Invoke(query).ToList() ?? query.ToList()).Take(fetchLimit);
                        return resultsWithLimit;
                    }

                    var results = orderBy?.Invoke(query).ToList() ?? query.ToList();
                    return results;
                }, new TimeSpan(0, 0, 0, 10));
                return result;
            }
            catch (Exception ex)
            {
                throw ExceptionHelpers.ThrowException(ex, true, "Fault in Repository");
            }
        }

        public void Add(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public void Add(params TEntity[] entities)
        {
            DbSet.AddRange(entities);
        }

        public void Add(IEnumerable<TEntity> entities)
        {
            DbSet.AddRange(entities);
        }

        public void Delete(TEntity entity)
        {
            var existing = DbSet.Find(entity);
            if (existing != null) DbSet.Remove(existing);
        }

        public void Delete(object id)
        {
            var typeInfo = typeof(TEntity).GetTypeInfo();
            var key = DbContext.Model.FindEntityType(typeInfo).FindPrimaryKey().Properties.FirstOrDefault();
            var property = typeInfo.GetProperty(key?.Name ?? throw new InvalidOperationException());
            if (property != null)
            {
                var entity = Activator.CreateInstance<TEntity>();
                property.SetValue(entity, id);
                DbContext.Entry(entity).State = EntityState.Deleted;
            }
            else
            {
                var entity = DbSet.Find(id);
                if (entity != null) Delete(entity);
            }
        }

        public void Delete(params TEntity[] entities)
        {
            DbSet.RemoveRange(entities);
        }

        public void Delete(IEnumerable<TEntity> entities)
        {
            DbSet.RemoveRange(entities);
        }

        public void Update(TEntity entity)
        {
            var trackedEntity = DbSet.Local.FirstOrDefault(e => e.Id.Equals(entity.Id));
            if (trackedEntity != null)
                this.DbContext.Entry(trackedEntity).State = EntityState.Detached;

            DbSet.Update(entity);
        }

        public void Update(params TEntity[] entities)
        {
            DbSet.UpdateRange(entities);
        }

        public void Update(IEnumerable<TEntity> entities)
        {
            DbSet.UpdateRange(entities);
        }

        public void Dispose()
        {
            DbContext?.Dispose();
        }
    }
}
