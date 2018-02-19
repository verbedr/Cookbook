using System;
using System.Threading.Tasks;
using Common.Repository;

namespace Common.Services
{
    public abstract class QueryHandler<TRequest, TResponse> : IHandler<TRequest, TResponse>
        where TRequest : class
        where TResponse : class
    {
        private readonly IUnitOfWork _context;

        protected QueryHandler(IUnitOfWork context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            _context = context;
        }

        public async Task<TResponse> HandleAsync(TRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            using (_context.AsReadOnly())
            {
                return await QueryAsync(request);
            }
        }

        protected abstract Task<TResponse> QueryAsync(TRequest request);
    }
}
