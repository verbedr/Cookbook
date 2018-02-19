namespace Cookbook.Contracts.Requests
{
    public class ItemRequest<T>
    {
        public T Id { get; set; }

        public bool IncludeDetails { get; set; }
    }
}
