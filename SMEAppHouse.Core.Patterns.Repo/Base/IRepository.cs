using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SMED.Core.Patterns.EF.ModelComposite;

namespace SMED.Core.Patterns.Repo.Base
{
    public interface IRepository<TEntity, in TPk>
        where TEntity : class, IIdentifiableEntity<TPk>
    {
        DbContext Context { get; set; }
        DbSet<TEntity> DbSet { get; set; }

        Task<TEntity> CreateAsync(TEntity entity, bool autoSave = false);
        Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false);


        Task RemoveAsync(TPk id, bool autoSave = false);
        Task RemoveAsync(TEntity entity, bool autoSave = false);
        Task RemoveAsync(Expression<Func<TEntity, bool>> filter, bool autoSave = false);

        Task RefreshAsync(TEntity entity);

        Task SaveAsync();

        int Count();
        int Count(Expression<Func<TEntity, bool>> filter);

        bool Any();
        bool Any(Expression<Func<TEntity, bool>> filter);



        Task<TEntity> GetAsync(TPk id);
        Task<TEntity> GetAsync(TPk id, string includeProperties);

        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAllAsync(string includeProperties);
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter);
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy);
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, int fetchLimit);
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, int fetchLimit, string includeProperties);

        Task<IEnumerable<TEntity>> GetAllAsync(int skip, int take);
        Task<IEnumerable<TEntity>> GetAllAsync(int skip, int take, Expression<Func<TEntity, bool>> filter);
        Task<IEnumerable<TEntity>> GetAllAsync(int skip, int take, Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy);
        Task<IEnumerable<TEntity>> GetAllAsync(int skip, int take, Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, string includeProperties);

        Task<IEnumerable<TEntity>> GetPageAsync(int pageNo, int pageSize);
        Task<IEnumerable<TEntity>> GetPageAsync(int pageNo, int pageSize, Expression<Func<TEntity, bool>> filter);
        Task<IEnumerable<TEntity>> GetPageAsync(int pageNo, int pageSize, Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy);
        Task<IEnumerable<TEntity>> GetPageAsync(int pageNo, int pageSize, Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, string includeProperties);

        Task<IEnumerable<TEntity>> GetWithSqlAsync(string query, params object[] parameters);
        Task<IEnumerable<TEntity>> GetWithSqlAsync(string query);

        Task<int> ExecuteNonQueryAsync(string query, params object[] parameters);
        Task<string> ExecuteQueryAsync(string query, params object[] parameters);


        Task<T> ExecuteSqlAsync<T>(string query, params object[] parameters) where T : 
            IComparable,
            IComparable<T>,
            IConvertible,
            IEquatable<T>,
            IFormattable;

        Task<T> ExecuteSqlAsync<T>(string query, string readField, params object[] parameters) where T : 
                                                                            IComparable,
                                                                            IComparable<T>,
                                                                            IConvertible,
                                                                            IEquatable<T>,
                                                                            IFormattable;

    }

}
