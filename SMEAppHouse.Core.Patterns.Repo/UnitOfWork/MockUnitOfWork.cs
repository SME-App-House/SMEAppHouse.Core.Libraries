using System;
using System.Collections.Generic;
using System.Linq;
using SMED.Core.Patterns.EF.ModelComposite;
using SMED.Core.Patterns.EFComposites;
using SMED.Core.Patterns.Repo.Asynchronous;

namespace SMED.Core.Patterns.Repo.UnitOfWork
{
    public class MockUnitOfWork<TContext> : IUnitOfWork where TContext : class//, new()
    {
        private readonly TContext _context;
        private readonly Dictionary<Type, object> _repositories;

        public MockUnitOfWork(TContext context)
        {
            _context = context;//new TContext();
            _repositories = new Dictionary<Type, object>();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TType"></typeparam>
        /// <returns></returns>
        public IRepository<TEntity, TType> GetRepository<TEntity, TType>()
            where TEntity : class, IIdentifiableEntity<TType>
            where TType : struct
        {
            if (_repositories.Keys.Contains(typeof(TEntity)))
                return _repositories[typeof(TEntity)] as IRepository<TEntity, TType>;
            
            MockRepository<TEntity, TType> repository = null;
            var entityName = typeof(TEntity).Name;
            var prop = _context.GetType().GetProperty(entityName);

            if (prop != null)
            {
                var entityValue = prop.GetValue(_context, null);
                repository = new MockRepository<TEntity, TType>(entityValue as List<TEntity>);
            }
            else
            {
                repository = new MockRepository<TEntity, TType>(new List<TEntity>());
            }
            _repositories.Add(typeof(TEntity), repository);
            return repository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TType"></typeparam>
        /// <param name="data"></param>
        public void SetRepository<TEntity, TType>(List<TEntity> data)
            where TEntity : class, IEntity<TType> 
            where TType : struct
        {
            var repo = GetRepository<TEntity, TType>();
            
            var mockRepo = repo as MockRepository<TEntity, TType>;
            if (mockRepo != null)
                mockRepo.Context = data;
        }

        public void Save()
        {
        }

        public void Dispose()
        {
        }
    }
}