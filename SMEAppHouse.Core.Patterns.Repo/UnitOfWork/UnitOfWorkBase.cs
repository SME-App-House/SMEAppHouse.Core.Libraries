using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SMEAppHouse.Core.Patterns.EF.ModelComposite;
using SMEAppHouse.Core.Patterns.Repo.Base;

namespace SMEAppHouse.Core.Patterns.Repo.UnitOfWork
{
    public class UnitOfWorkBase<TContext> : IUnitOfWork
        where TContext : DbContext, new()
    {
        private readonly TContext _context;
        private readonly Dictionary<Type, object> _repositories;
        private bool _disposed;

        #region constructor
        public UnitOfWorkBase()
            : this(null)
        {
        }

        // Default constructor that news the context and the dictionary containing all the repositories
        public UnitOfWorkBase(TContext context)
        {
            _context = context ?? new TContext();
            _repositories = new Dictionary<Type, object>();
            _disposed = false;
        }

        #endregion

        /// <summary>
        /// Retrieves the repository for some Model class T
        /// If it doesn't exist, we create an instance of it
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TPk"></typeparam>
        /// <returns></returns>
        public IRepositorySync<TEntity, TPk> GetRepository<TEntity, TPk>()
            where TEntity : class, IIdentifiableEntity<TPk>
            where TPk : struct
        {
            // Checks if the Dictionary Key contains the Model class
            if (_repositories.Keys.Contains(typeof(TEntity)))
            {
                // Return the repository for that Model class
                return _repositories[typeof(TEntity)] as IRepositorySync<TEntity, TPk>;
            }

            // If the repository for that Model class doesn't exist, then create it
            var repository = new RepositoryBaseSync<TEntity, TPk, TContext>(_context);

            // Add it to the dictionary
            _repositories.Add(typeof(TEntity), repository);

            // Return it
            return repository;
        }

        /// <summary>
        /// Saves all changes done to the context
        /// </summary>
        public void Save()
        {
            _context.SaveChanges(); //.SubmitChanges();
        }

        /// <summary>
        /// Disposes the context
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the context only once if not disposed
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing) _context.Dispose();
            _disposed = true;
        }

    }
}