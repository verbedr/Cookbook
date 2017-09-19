namespace Common.Services
{
    public interface IRequestHandlerFactory
    {
        IRequestHandler<TSource, TTarget> Resolve<TSource, TTarget>()
            where TSource : class
            where TTarget : class;

        void Release<TSource, TTarget>(IRequestHandler<TSource, TTarget> item)
            where TTarget : class
            where TSource : class;
    }
}
