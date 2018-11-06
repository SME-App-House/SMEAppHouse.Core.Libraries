using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SMED.Core.CodeKits.Exceptions;
using SMED.Core.CodeKits.Tools;
using SMED.Core.Patterns.EF.ModelComposite;
using SMED.Core.Patterns.EFComposites;
using SMED.Core.Patterns.Repo.Base;

namespace SMED.Core.Patterns.Repo.Asynchronous
{
    public class RepositoryBase<TEntity, TPk, TDbContext> : IRepositoryAsync<TEntity, TPk>, IDisposable
        where TEntity : class, IIdentifiableEntity<TPk>
        where TDbContext : DbContext, new()
    {

        [Required]
        public DbContext Context { get; set; }
        public DbSet<TEntity> DbSet { get; set; }

        #region constructors

        public RepositoryBase()
            : this(new TDbContext())
        {
        }

        public RepositoryBase(TDbContext context)
        {
            Context = context;
            DbSet = context.Set<TEntity>();
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="autoSave"></param>
        public virtual TEntity Create(TEntity entity, bool autoSave = false)
        {
            try
            {
                DbSet.Add(entity);
                if (autoSave)
                {
                    Save();
                    return entity;
                }
            }
            catch (Exception ex)
            {
                throw ExceptionHelpers.ThrowException(ex, true, "Fault in Repository");
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="autoSave"></param>
        public void Update(TEntity entity, bool autoSave = false)
        {
            try
            {
                entity.DateRevised = DateTime.Now;
                DbSet.Attach(entity);
                Context.Entry(entity).State = EntityState.Modified;
                if (autoSave) Save();
            }
            catch (Exception ex)
            {
                throw ExceptionHelpers.ThrowException(ex, true, "Fault in Repository");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="autoSave"></param>
        public void Remove(TPk id, bool autoSave = false)
        {
            try
            {
                var entity = DbSet.Find(id);
                Remove(entity);
                if (autoSave) Save();
            }
            catch (Exception ex)
            {
                throw ExceptionHelpers.ThrowException(ex, true, "Fault in Repository");
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="autoSave"></param>
        public void Remove(TEntity entity, bool autoSave = false)
        {
            try
            {
                if (Context.Entry(entity).State == EntityState.Detached)
                    DbSet.Attach(entity);

                DbSet.Remove(entity);
                if (autoSave) Save();
            }
            catch (Exception ex)
            {
                throw ExceptionHelpers.ThrowException(ex, true, "Fault in Repository");
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="autoSave"></param>
        public void Remove(Expression<Func<TEntity, bool>> filter, bool autoSave = false)
        {
            try
            {
                IQueryable<TEntity> query = DbSet;
                var members = query.Where(filter);

                foreach (var m in members)
                    Remove(m);

                if (autoSave) Save();
            }
            catch (Exception ex)
            {
                throw ExceptionHelpers.ThrowException(ex, true, "Fault in Repository");
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void Refresh(TEntity entity)
        {
            try
            {
                Context.Entry(entity).Reload();
            }
            catch (Exception ex)
            {
                throw ExceptionHelpers.ThrowException(ex, true, "Fault in Repository");
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public void Save()
        {
            try
            {
                Context.SaveChanges();
            }
            //catch (DbEntityValidationException ex)
            //{
            //    // Retrieve the error messages as a list of strings.
            //    var errorMessages = ex.EntityValidationErrors
            //            .SelectMany(x => x.ValidationErrors)
            //            .Select(x => x.ErrorMessage);

            //    // Join the list to a single string.
            //    var fullErrorMessage = string.Join("; ", errorMessages);

            //    // Combine the original exception message with the new one.
            //    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

            //    // Throw a new DbEntityValidationException with the improved exception message.
            //    throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            //}
            catch (Exception ex)
            {
                throw ExceptionHelpers.ThrowException(ex, true, "Fault in Repository");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return Count(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public int Count(Expression<Func<TEntity, bool>> filter)
        {
            try
            {
                var cnt = 0;
                Exception exception = null;

                if (RetryCodeKit.DoWhileError(
                    () =>
                    {
                        cnt = filter == null ? DbSet.Count() : DbSet.Count(filter);
                    },
                    (int retryCtr, Exception ex, ref bool cancelExec) =>
                    {
                        exception = ex;
                    }, 3, 5))
                    return cnt;

                if (exception != null)
                    throw exception;

                return cnt;
            }
            catch (Exception ex)
            {
                throw ExceptionHelpers.ThrowException(ex, true, "Fault in Repository");
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Any()
        {
            return Any(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public bool Any(Expression<Func<TEntity, bool>> filter)
        {
            try
            {
                return Count(filter) > 0;
            }
            catch (Exception ex)
            {
                throw ExceptionHelpers.ThrowException(ex, true, "Fault in Repository");
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TEntity Get(TPk id)
        {
            try
            {
                return DbSet.Find(id);
            }
            catch (Exception ex)
            {
                throw ExceptionHelpers.ThrowException(ex, true, "Fault in Repository");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        public TEntity Get(TPk id, string includeProperties)
        {
            var result = GetAll(p => p.Id.Equals(id), null, 0, includeProperties)?.FirstOrDefault();
            return result;
        }

        #region ordinary get

        public IEnumerable<TEntity> GetAll()
        {
            try
            {
                return DbSet.ToList();
            }
            catch (Exception ex)
            {
                throw ExceptionHelpers.ThrowException(ex, true, "Fault in Repository");
            }
        }

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter)
        {
            return GetAll(filter, null, 0, string.Empty);
        }

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter,
                                        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy)
        {
            return GetAll(filter, orderBy, 0, string.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="fetchLimit"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            int fetchLimit)
        {
            return GetAll(filter, orderBy, fetchLimit, string.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="fetchLimit"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter,
                                        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
                                        int fetchLimit,
                                        string includeProperties)
        {
            try
            {
                var result = RetryCodeKit.Do(() =>
                            {
                                try
                                {
                                    IQueryable<TEntity> query = DbSet;

                                    if (filter != null)
                                        query = query.Where(filter);

                                    if (!string.IsNullOrEmpty(includeProperties))
                                        query = includeProperties
                                            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                            .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

                                    if (fetchLimit > 0)
                                    {
                                        var resultsWithLimit = (orderBy?.Invoke(query).ToList() ?? query.ToList()).Take(fetchLimit);
                                        return resultsWithLimit;
                                    }

                                    var results = orderBy?.Invoke(query).ToList() ?? query.ToList();
                                    return results;
                                }
                                catch (Exception ex)
                                {
                                    throw;
                                }
                            }, new TimeSpan(0, 0, 0, 30));

                return result;

            }
            catch (Exception ex)
            {
                throw ExceptionHelpers.ThrowException(ex, true, "Fault in Repository");
            }



        }

        public IEnumerable<TEntity> GetAll(string includeProperties)
        {
            try
            {
                var result = RetryCodeKit.Do(() =>
                            {
                                try
                                {
                                    IQueryable<TEntity> query = DbSet;

                                    if (!string.IsNullOrEmpty(includeProperties))
                                        query = includeProperties
                                            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                            .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

                                    var results = query.ToList();

                                    return results;
                                }
                                catch (Exception ex)
                                {
                                    throw;
                                }
                            }, new TimeSpan(0, 0, 0, 30));

                return result;
            }
            catch (Exception ex)
            {
                throw ExceptionHelpers.ThrowException(ex, true, "Fault in Repository");
            }
        }

        #endregion

        #region get by targetting

        public IEnumerable<TEntity> GetAll(int skip, int take)
        {
            return GetAll(skip, take, null, null, string.Empty);
        }

        public IEnumerable<TEntity> GetAll(int skip, int take, Expression<Func<TEntity, bool>> filter)
        {
            return GetAll(skip, take, filter, null, string.Empty);
        }

        public IEnumerable<TEntity> GetAll(int skip, int take, Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy)
        {
            return GetAll(skip, take, filter, orderBy, string.Empty);
        }

        public IEnumerable<TEntity> GetAll(int skip, int take, Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, string includeProperties)
        {
            try
            {
                var result = RetryCodeKit.Do(() =>
                           {
                               try
                               {
                                   IQueryable<TEntity> query = DbSet;

                                   if (filter != null)
                                   {
                                       query = query.Where(filter);
                                   }

                                   query = includeProperties
                                       .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                       .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));


                                   return orderBy?.Invoke(query).Skip(skip).Take(take).ToList() ?? query.OrderBy(e => e.Id).Skip(skip).Take(take).ToList();
                               }
                               catch (Exception ex)
                               {
                                   throw;
                               }
                           }, new TimeSpan(0, 0, 0, 30));

                return result;
            }
            catch (Exception ex)
            {
                throw ExceptionHelpers.ThrowException(ex, true, "Fault in Repository");
            }


        }

        #endregion

        #region get via paging

        public IEnumerable<TEntity> GetPage(int pageNo, int pageSize)
        {
            return GetPage(pageNo, pageSize, null, null);
        }

        public IEnumerable<TEntity> GetPage(int pageNo
                                        , int pageSize
                                        , Expression<Func<TEntity, bool>> filter)
        {
            return GetPage(pageNo, pageSize, filter, null);
        }

        public IEnumerable<TEntity> GetPage(int pageNo
                                        , int pageSize
                                        , Expression<Func<TEntity, bool>> filter
                                        , Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy)
        {
            return GetPage(pageNo, pageSize, filter, orderBy, string.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> GetPage(int pageNo, int pageSize, Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, string includeProperties)
        {

            Exception exception = null;
            List<TEntity> entities = null;

            try
            {
                if (!RetryCodeKit.LoopRetry(
                                retryAction: () =>
                                {
                                    try
                                    {
                                        IQueryable<TEntity> query = DbSet;

                                        if (filter != null)
                                            query = query.Where(filter);

                                        if (!string.IsNullOrEmpty(includeProperties))
                                            query = includeProperties
                                                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

                                        entities = orderBy?.Invoke(query).Skip(pageNo * pageSize).Take(pageSize).ToList() ??
                                                query.OrderBy(e => e.Id).Skip(pageNo * pageSize).Take(pageSize).ToList();

                                        return true;
                                    }
                                    catch (Exception ex)
                                    {
                                        throw;
                                    }
                                },
                                successQualifier: () => entities != null,
                                extraRoutineAfterFailure: null,
                                exceptionThrown: ref exception,
                                iterationLimit: 3,
                                iterationTimeout: 5000))
                {
                    if (exception != null)
                        throw exception;
                }
            }
            catch (Exception ex)
            {
                throw ExceptionHelpers.ThrowException(ex, true, "Fault in Repository");
            }

            return entities;
        }

        #endregion

        #region Get with Raw SQL

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> GetWithSql(string query, params object[] parameters)
        {
            try
            {
                return DbSet.FromSql(query, parameters).ToList();
            }
            catch (Exception ex)
            {
                throw ExceptionHelpers.ThrowException(ex, true, "Fault in Repository");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> GetWithSql(string query)
        {
            try
            {
                var result = DbSet.FromSql(query);
                return result.ToList();
            }
            catch (Exception ex)
            {
                throw ExceptionHelpers.ThrowException(ex, true, "Fault in Repository");
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string query, params object[] parameters)
        {
            try
            {
                using (var context = new SqlConnection(this.Context.Database.GetDbConnection().ConnectionString))
                {
                    var cmd = new SqlCommand
                    {
                        CommandText = query,
                        CommandType = CommandType.Text,
                        Connection = context
                    };
                    if (parameters.Any())
                        cmd.Parameters.AddRange(parameters);
                    context.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ExceptionHelpers.ThrowException(ex, true, "Fault in Repository");
            }
        }

        /// <summary>
        /// Executes a scalar query which returns the first column of the first row as string.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string ExecuteQuery(string query, params object[] parameters)
        {
            try
            {
                using (var context = new SqlConnection(this.Context.Database.GetDbConnection().ConnectionString))
                {
                    var cmd = new SqlCommand
                    {
                        CommandText = query,
                        CommandType = CommandType.Text,
                        Connection = context
                    };
                    if (parameters.Any())
                        cmd.Parameters.AddRange(parameters);
                    context.Open();
                    return (string)cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                throw ExceptionHelpers.ThrowException(ex, true, "Fault in Repository");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public T ExecuteSql<T>(string query, params object[] parameters)
            where T : IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            return ExecuteSql<T>(query, string.Empty, parameters);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="readField"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public T ExecuteSql<T>(string query, string readField, params object[] parameters)
            where T : IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            try
            {
                using (var command = this.Context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = query;
                    this.Context.Database.OpenConnection();
                    if (string.IsNullOrEmpty(readField))
                    {
                        var result = (T)command.ExecuteScalar();
                        return result;
                    }
                    else
                    {
                        using (var reader = command.ExecuteReader())
                            return (T)reader[readField];
                    }
                }

                //using (var conn = new SqlConnection(this.Context.Database.GetDbConnection().ConnectionString))
                //{
                //    var cmd = new SqlCommand
                //    {
                //        CommandText = query,
                //        CommandType = CommandType.Text,
                //        Connection = conn
                //    };
                //    if (parameters.Any())
                //        cmd.Parameters.AddRange(parameters);
                //    conn.Open();

                //    if (string.IsNullOrEmpty(readField))
                //    {
                //        var result = (T)cmd.ExecuteScalar();
                //        return result;
                //    }
                //    else
                //    {
                //        using (var reader = cmd.ExecuteReader())
                //        {
                //            return (T)reader[readField];
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                throw ExceptionHelpers.ThrowException(ex, true, "Fault in Repository");
            }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Context.Dispose();
        }


    }
}