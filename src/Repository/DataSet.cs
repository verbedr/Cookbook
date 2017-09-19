using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Common.Domain;
using Common.Repository;

namespace Cookbook.Repository
{
    class DataSet<TEntity> : IDataSet<TEntity> where TEntity : Entity
    {
        private readonly CookbookDbContext _dbContext;

        public DataSet(CookbookDbContext dbContext)
        {
            if (dbContext == null) throw new ArgumentNullException(nameof(dbContext));

            _dbContext = dbContext;
        }

        public TEntity this[int id]
        {
            get { return Query().SingleOrDefault(x => x.Id == id); }
        }

        public IQueryable<TEntity> Query()
        {
            return _dbContext.Set<TEntity>().AsQueryable();
        }

        public IQueryable<TEntity> QueryWithNoTracking()
        {
            return _dbContext.Set<TEntity>().AsNoTracking();
        }


        public TEntity Add(TEntity entity)
        {
            if (_dbContext.IsReadOnly)
                throw new NotSupportedException("In readonly state");

            return _dbContext.Set<TEntity>().Add(entity);
        }

        public IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities)
        {
            if (_dbContext.IsReadOnly)
                throw new NotSupportedException("In readonly state");

            return _dbContext.Set<TEntity>().AddRange(entities);
        }

        public TEntity Remove(TEntity entity)
        {
            if (_dbContext.IsReadOnly)
                throw new NotSupportedException("In readonly state");

            return _dbContext.Set<TEntity>().Remove(entity);
        }

        public IEnumerable<TEntity> RemoveRange(IEnumerable<TEntity> entities)
        {
            if (_dbContext.IsReadOnly)
                throw new NotSupportedException("In readonly state");

            return _dbContext.Set<TEntity>().RemoveRange(entities);
        }

        public TEntity Attach(TEntity entity)
        {
            if (_dbContext.IsReadOnly)
                throw new NotSupportedException("In readonly state");

            return _dbContext.Set<TEntity>().Attach(entity);
        }

        public TEntity Create()
        {
            if (_dbContext.IsReadOnly)
                throw new NotSupportedException("In readonly state");

            return _dbContext.Set<TEntity>().Create();
        }

        public TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, TEntity
        {
            if (_dbContext.IsReadOnly)
                throw new NotSupportedException("In readonly state");

            return _dbContext.Set<TEntity>().Create<TDerivedEntity>();
        }

        public async Task<TEntity> FindAsync(params object[] keyValues)
        {
            return await _dbContext.Set<TEntity>().FindAsync(keyValues);
        }

        public void Ensure<TElement>(TEntity entity, Expression<Func<TEntity, ICollection<TElement>>> p) where TElement : class
        {
            if (_dbContext.Entry(entity).State == System.Data.Entity.EntityState.Added) return;
            _dbContext.Entry(entity).Collection(p).Load();
        }
    }
}
