using Castle.Core.Logging;
using Castle.MicroKernel;
using Castle.MicroKernel.Lifestyle;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Common.Services;

namespace Cookbook.Castle
{
    internal class RequestHandlerMediator : IRequestHandlerMediator
    {
        private const string InfoMessage = "Handled request for {0} ({1} ms).";
        private const string WarnMessage = "Request for {0} caused a business exception ({1} ms).";
        private const string ErrorMessage = "Request for {0} caused an exception ({1} ms).";

        private readonly IKernel _container;
        private readonly IRequestHandlerFactory _handlerFactory;
        public ILogger Log { get; set; } = NullLogger.Instance;

        public RequestHandlerMediator(IKernel container, IRequestHandlerFactory handlerFactory)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
            _handlerFactory = handlerFactory ?? throw new ArgumentNullException(nameof(handlerFactory));
        }

        public async Task<TResponse> ExecuteAsync<TRequest, TResponse>(TRequest request) where TRequest : class where TResponse : class
        {
            var requestType = typeof(TRequest).FullName.Replace("Softwel.MijnOrbis.Contracts.Requests.", "");
            var stopwatch = new Stopwatch();
            IRequestHandler<TRequest, TResponse> handler = null;

            using (_container.BeginScope())
            {
                try
                {
                    stopwatch.Start();
                    handler = _handlerFactory.Resolve<TRequest, TResponse>();
                    var returnValue = await handler.HandleAsync(request);
                    stopwatch.Stop();
                    Log.InfoFormat(InfoMessage, requestType, stopwatch.ElapsedMilliseconds);
                    return returnValue;
                }
                catch (Common.ApiException orbisEx)
                {
                    Log.WarnFormat(orbisEx, WarnMessage, requestType, stopwatch.ElapsedMilliseconds);
                    throw;
                }
                catch (Exception ex)
                {
                    Log.ErrorFormat(ex, WarnMessage, requestType, stopwatch.ElapsedMilliseconds);
                    throw;
                }
                finally
                {
                    if (handler != null)
                    {
                        _handlerFactory.Release(handler);
                    }
                }
            }
        }
    }
}