using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SMEAppHouse.Core.Patterns.EF.ModelComposite;

namespace SMEAppHouse.Core.Patterns.Repo.Base
{
    /// <summary>
    /// Synchronous repository
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TPk"></typeparam>
    public interface IRepositorySync<TEntity, in TPk>
        where TEntity : class, IIdentifiableEntity<TPk>
    {
        DbContext Context { get; set; }
        DbSet<TEntity> DbSet { get; set; }

        TEntity Create(TEntity entity, bool autoSave = false);
        void Update(TEntity entity, bool autoSave = false);


        void Remove(TPk id, bool autoSave = false);
        void Remove(TEntity entity, bool autoSave = false);
        void Remove(Expression<Func<TEntity, bool>> filter, bool autoSave = false);

        void Refresh(TEntity entity);

        void Save();

        int Count();
        int Count(Expression<Func<TEntity, bool>> filter);

        bool Any();
        bool Any(Expression<Func<TEntity, bool>> filter);



        TEntity Get(TPk id);
        TEntity Get(TPk id, string includeProperties);

        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetAll(string includeProperties);
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter);
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy);
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, int fetchLimit);
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, int fetchLimit, string includeProperties);

        IEnumerable<TEntity> GetAll(int skip, int take);
        IEnumerable<TEntity> GetAll(int skip, int take, Expression<Func<TEntity, bool>> filter);
        IEnumerable<TEntity> GetAll(int skip, int take, Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy);
        IEnumerable<TEntity> GetAll(int skip, int take, Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, string includeProperties);

        IEnumerable<TEntity> GetPage(int pageNo, int pageSize);
        IEnumerable<TEntity> GetPage(int pageNo, int pageSize, Expression<Func<TEntity, bool>> filter);
        IEnumerable<TEntity> GetPage(int pageNo, int pageSize, Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy);
        IEnumerable<TEntity> GetPage(int pageNo, int pageSize, Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, string includeProperties);

        IEnumerable<TEntity> GetWithSql(string query, params object[] parameters);
        IEnumerable<TEntity> GetWithSql(string query);

        int ExecuteNonQuery(string query, params object[] parameters);
        string ExecuteQuery(string query, params object[] parameters);


        T ExecuteSql<T>(string query, params object[] parameters) where T : 
            IComparable,
            IComparable<T>,
            IConvertible,
            IEquatable<T>,
            IFormattable;

        T ExecuteSql<T>(string query, string readField, params object[] parameters) where T : 
                                                                            IComparable,
                                                                            IComparable<T>,
                                                                            IConvertible,
                                                                            IEquatable<T>,
                                                                            IFormattable;

    }

}
