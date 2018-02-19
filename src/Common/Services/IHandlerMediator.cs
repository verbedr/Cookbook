using System.Threading.Tasks;

namespace Common.Services
{
    public interface IHandlerMediator
    {
        Task<TResponse> ExecuteAsync<TRequest, TResponse>(TRequest request)
            where TRequest : class
            where TResponse : class;
    }
}
