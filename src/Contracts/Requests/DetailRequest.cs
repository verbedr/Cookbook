namespace Cookbook.Contracts.Requests
{
    public abstract class DetailRequest<TKey, T> : ItemRequest<TKey>
    {
        protected DetailRequest() { }

        public TKey Detail { get; set; }
    }
}
