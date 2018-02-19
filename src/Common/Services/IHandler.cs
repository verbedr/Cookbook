using System.Threading.Tasks;

namespace Common.Services
{
    public interface IHandler<in TRequest, TResponse>
        where TRequest : class
        where TResponse : class
    {
        Task<TResponse> HandleAsync(TRequest request);
    }
}
