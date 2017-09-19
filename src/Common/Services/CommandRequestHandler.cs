using System;
using System.Threading.Tasks;
using Common.Repository;

namespace Common.Services
{
    public abstract class CommandRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : class
        where TResponse : class
    {
        private readonly IUnitOfWork _context;

        protected IUnitOfWork Context { get { return _context; } }

        protected CommandRequestHandler(IUnitOfWork context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            _context = context;
        }

        public async Task<TResponse> HandleAsync(TRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            using (var scope = _context.BeginTransaction())
            {
                var returnValue = await ExecuteAsync(request);

                await scope.CompletedAsync();
                return returnValue;
            }
        }

        protected abstract Task<TResponse> ExecuteAsync(TRequest request);
    }
}
