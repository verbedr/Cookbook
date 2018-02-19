namespace Common.Services
{
    public interface IHandlerFactory
    {
        IHandler<TSource, TTarget> Resolve<TSource, TTarget>()
            where TSource : class
            where TTarget : class;

        void Release<TSource, TTarget>(IHandler<TSource, TTarget> item)
            where TTarget : class
            where TSource : class;
    }
}
