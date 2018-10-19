namespace Cookbook.Contracts.Responses
{
    public abstract class ListResponse<T>
    {
        protected ListResponse() { }

        public T[] Items { get; set; }
    }
}
