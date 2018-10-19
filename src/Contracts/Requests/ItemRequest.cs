namespace Cookbook.Contracts.Requests
{
    public abstract class ItemRequest<T> : BaseRequest
    {
        protected ItemRequest() { }

        public T Id { get; set; }
    }
}
