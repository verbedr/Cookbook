using Common.Domain;
using Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.Domain
{
    public class CookbookFactory
    {
        private readonly IDataSetFactory _factory;

        public CookbookFactory(IDataSetFactory factory)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            _factory = factory;
            Instance = this;
        }

        public static CookbookFactory Instance { get; internal set; }

        public virtual DateTime Now => DateTime.Now;

        #region Helpers

        protected void PerformWithRepository<TEntity>(Action<IDataSet<TEntity>> action) where TEntity : IEntity
        {
            IDataSet<TEntity> repository = _factory.Resolve<TEntity>();
            try
            {
                action(repository);
            }
            finally
            {
                if (repository != null)
                {
                    _factory.Dispose(repository);
                }
            }
        }

        protected TResult PerformWithRepository<TEntity, TResult>(Func<IDataSet<TEntity>, TResult> action)
            where TEntity : IEntity
        {
            IDataSet<TEntity> repository = _factory.Resolve<TEntity>();
            try
            {
                return action(repository);
            }
            finally
            {
                if (repository != null)
                {
                    _factory.Dispose(repository);
                }
            }
        }

        protected async Task PerformWithRepositoryAsync<TEntity>(Func<IDataSet<TEntity>, Task> action) where TEntity : Entity
        {
            IDataSet<TEntity> repository = _factory.Resolve<TEntity>();
            try
            {
                await action(repository);
            }
            finally
            {
                if (repository != null)
                {
                    _factory.Dispose(repository);
                }
            }
        }

        protected async Task<TResult> PerformWithRepositoryAsync<TEntity, TResult>(Func<IDataSet<TEntity>, Task<TResult>> action)
            where TEntity : IEntity
        {
            IDataSet<TEntity> repository = _factory.Resolve<TEntity>();
            try
            {
                return await action(repository);
            }
            finally
            {
                if (repository != null)
                {
                    _factory.Dispose(repository);
                }
            }
        }

        #endregion

        public virtual IQueryable<TEntity> Query<TEntity>() where TEntity : IEntity
        {
            return PerformWithRepository<TEntity, IQueryable<TEntity>>(r => r.Query());
        }

        public async virtual Task<TEntity> FindAsync<TEntity>(int key) where TEntity : IEntity
        {
            return await PerformWithRepositoryAsync<TEntity, TEntity>(async r => await r.FindAsync(key));
        }

        public virtual void Add<TEntity>(TEntity item) where TEntity : IEntity
        {
            PerformWithRepository<TEntity>(r => r.Add(item));
        }

        public virtual void AddRange<TEntity>(IEnumerable<TEntity> items) where TEntity : IEntity
        {
            PerformWithRepository<TEntity>(r => r.AddRange(items));
        }

        public virtual void Remove<TEntity>(TEntity item) where TEntity : IEntity
        {
            PerformWithRepository<TEntity>(r => r.Remove(item));
        }

        public virtual void RemoveRange<TEntity>(IEnumerable<TEntity> items) where TEntity : IEntity
        {
            PerformWithRepository<TEntity>(r => r.RemoveRange(items));
        }

        public void Ensure<TEntity, TElement>(TEntity entity, Expression<Func<TEntity, ICollection<TElement>>> p)
            where TEntity : IEntity
            where TElement : class
        {
            PerformWithRepository<TEntity>(r => r.Ensure(entity, p));
        }
    }
}
