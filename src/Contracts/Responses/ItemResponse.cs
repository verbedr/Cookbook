namespace Cookbook.Contracts.Responses
{
    public abstract class ItemResponse<T>
    {
        protected ItemResponse() { }

        public T Item { get; set; }
    }
}
