using System;
using System.Threading.Tasks;

namespace Common.Repository
{
    public interface IUnitOfWorkTransaction : IDisposable
    {
        Task CompletedAsync();
    }
}
