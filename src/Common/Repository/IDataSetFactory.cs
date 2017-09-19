using Common.Domain;

namespace Common.Repository
{
    public interface IDataSetFactory
    {
        IDataSet<TEntity> Resolve<TEntity>() where TEntity : IEntity;
        void Dispose<TEntity>(IDataSet<TEntity> item) where TEntity : IEntity;
    }
}
