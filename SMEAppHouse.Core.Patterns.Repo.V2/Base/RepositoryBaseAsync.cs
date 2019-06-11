using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SMEAppHouse.Core.Patterns.Repo.V2.Base
{
    public class RepositoryBaseAsync<TEntity, TPk, TDbContext> : IRepositoryAsync<TEntity, TPk>, IDisposable
        where TEntity : class, IIdentifiableEntity<TPk>
        where TDbContext : DbContext, new()
    {
        public DbContext Context { get; set; }
        public DbSet<TEntity> DbSet { get; set; }
        public Task<TEntity> CreateAsync(TEntity entity, bool autoSave = false)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(TPk id, bool autoSave = false)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(TEntity entity, bool autoSave = false)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(Expression<Func<TEntity, bool>> filter, bool autoSave = false)
        {
            throw new NotImplementedException();
        }

        public Task RefreshAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync()
        {
            throw new NotImplementedException();
        }

        public int Count()
        {
            throw new NotImplementedException();
        }

        public int Count(Expression<Func<TEntity, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public bool Any()
        {
            throw new NotImplementedException();
        }

        public bool Any(Expression<Func<TEntity, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> GetAsync(TPk id)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> GetAsync(TPk id, string includeProperties)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetAllAsync(string includeProperties)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, int fetchLimit)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, int fetchLimit, string includeProperties)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetAllAsync(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetAllAsync(int skip, int take, Expression<Func<TEntity, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetAllAsync(int skip, int take, Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetAllAsync(int skip, int take, Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, string includeProperties)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetPageAsync(int pageNo, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetPageAsync(int pageNo, int pageSize, Expression<Func<TEntity, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetPageAsync(int pageNo, int pageSize, Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetPageAsync(int pageNo, int pageSize, Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, string includeProperties)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetWithSqlAsync(string query, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetWithSqlAsync(string query)
        {
            throw new NotImplementedException();
        }

        public Task<int> ExecuteNonQueryAsync(string query, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public Task<string> ExecuteQueryAsync(string query, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public Task<T> ExecuteSqlAsync<T>(string query, params object[] parameters) where T : IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            throw new NotImplementedException();
        }

        public Task<T> ExecuteSqlAsync<T>(string query, string readField, params object[] parameters) where T : IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}