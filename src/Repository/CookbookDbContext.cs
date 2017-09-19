using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Threading.Tasks;
using Cookbook.Domain;
using System.Data.Entity.Infrastructure;
using Common.Repository;

namespace Cookbook.Repository
{
    internal class CookbookDbContext : DbContext, IUnitOfWork, ICookbookRepository
    {
        public CookbookDbContext() : base()
        {
            Initiate(false);
        }

        public CookbookDbContext(ConnectionOptions configuration) : base(configuration.ConnectionString)
        {
            Initiate(configuration.EnableDbContextMigration);
        }

        private void Initiate(bool enableMigration)
        {
            Configuration.LazyLoadingEnabled = true;
            Configuration.ValidateOnSaveEnabled = false; // otherwise required properties will fail
            if (enableMigration)
            {
                Database.SetInitializer(new MigrateDatabaseToLatestVersion<CookbookDbContext, Migrations.Configuration>(true));
            }
        }


        #region Configuration
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            ConfigureBuilder(modelBuilder);
        }

        private static void ConfigureBuilder(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.AddFromAssembly(typeof(CookbookDbContext).Assembly);
        }

        #endregion

        #region Unit Of Work Implementation

        public bool IsReadOnly { get; private set; }

        public IUnitOfWorkTransaction AsReadOnly()
        {
            IsReadOnly = true;
            return new UnitOfWorkTransaction(this, IsReadOnly, false);
        }

        public IUnitOfWorkTransaction BeginTransaction(bool isLongRunning = false)
        {
            IsReadOnly = false;
            return new UnitOfWorkTransaction(this, IsReadOnly, isLongRunning);
        }

        private sealed class UnitOfWorkTransaction : IUnitOfWorkTransaction
        {
            private readonly CookbookDbContext _dataModelContext;
            private readonly DbContextTransaction _transaction;
            private readonly bool _readOnly;

            public UnitOfWorkTransaction(CookbookDbContext dataModelContext, bool readOnly, bool isLongRunning)
            {
                _dataModelContext = dataModelContext ?? throw new ArgumentNullException(nameof(dataModelContext));
                _readOnly = readOnly;
                if (isLongRunning) return;
                _transaction = _dataModelContext.Database.BeginTransaction();
            }

            public async Task CompletedAsync()
            {
                if (_readOnly) return;
                _dataModelContext.ChangeTracker.DetectChanges();
                await _dataModelContext.SaveChangesAsync();
                _transaction?.Commit();
            }

            #region IDisposable
            private bool _disposed;

            public void Dispose()
            {
                Dispose(true);

                // Call SupressFinalize in case a subclass implements a finalizer.
                GC.SuppressFinalize(this);
            }

            private void Dispose(bool disposing)
            {
                if (_disposed) return;

                // If you need thread safety, use a lock around these  
                // operations, as well as in your methods that use the resource. 
                if (disposing)
                {
                    _transaction?.Dispose();
                }

                // Indicate that the instance has been disposed.
                _disposed = true;
            }
            #endregion

        }

        #region ConceptionalNullHandler

        private readonly Dictionary<Type, IConceptionalNullHandler> _conceptionalNulls =
            new Dictionary<Type, IConceptionalNullHandler>();

        private interface IConceptionalNullHandler
        {
            void RemoveIfConceptionalNullsFound(object item);
        }

        private class ConceptionaNullHandler<T> : IConceptionalNullHandler where T : class
        {
            private readonly DbContext _context;
            private readonly Predicate<T> _predicate;

            public ConceptionaNullHandler(DbContext context, Predicate<T> predicate)
            {
                _context = context;
                _predicate = predicate;
            }

            public void RemoveIfConceptionalNullsFound(object item)
            {
                var asType = item as T;
                if (asType != null && _predicate(asType))
                {
                    _context.Set<T>().Remove(asType);
                }
            }
        }

        private void RemoveConceptionalNulledEntities()
        {
            var entries = ChangeTracker.Entries().Where(x => x.State == EntityState.Modified);
            Configuration.AutoDetectChangesEnabled = false;
            foreach (var entry in entries)
            {
                IConceptionalNullHandler handler;
                if (_conceptionalNulls.TryGetValue(ObjectContext.GetObjectType(entry.Entity.GetType()), out handler))
                {
                    handler.RemoveIfConceptionalNullsFound(entry.Entity);
                }
            }
            Configuration.AutoDetectChangesEnabled = true;
        }

        #endregion

        public async override Task<int> SaveChangesAsync()
        {
            RemoveConceptionalNulledEntities();
            return await base.SaveChangesAsync();
        }

        #endregion
        
    }
}
