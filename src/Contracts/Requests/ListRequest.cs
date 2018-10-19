namespace Cookbook.Contracts.Requests
{
    public abstract class ListRequest : BaseRequest
    {
        protected ListRequest() { }

        public int Skip { get; set; } = 0;

        public int Limit { get; set; } = 100;
        
        public string Sort { get; set; } 
    }
}
