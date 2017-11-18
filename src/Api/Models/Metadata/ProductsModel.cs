using Cookbook.Contracts.PropertyBags;

namespace Cookbook.Api.Models.Metadata
{
    public class ProductsModel
    {
        public ProductsModelEmbedded _embedded { get; set; }

        public class ProductsModelEmbedded
        {
            public ProductSummary[] Item { get; set; }
        }
    }
}
