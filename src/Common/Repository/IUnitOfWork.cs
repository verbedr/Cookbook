using System.Threading.Tasks;

namespace Common.Repository
{
    public interface IUnitOfWork
    {
        IUnitOfWorkTransaction AsReadOnly();
        IUnitOfWorkTransaction BeginTransaction(bool isLongRunning = false);
        Task<int> SaveChangesAsync();
    }
}
