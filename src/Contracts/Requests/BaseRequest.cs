namespace Cookbook.Contracts.Requests
{
    public abstract class BaseRequest
    {
        protected BaseRequest() { }

        public bool IncludeDetails { get; set; }
    }
}
