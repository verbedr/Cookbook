using Cookbook.Contracts.Requests.MetadataRequests;
using Cookbook.Contracts.Responses.MetadataResponses;
using System.Threading.Tasks;

namespace Cookbook.Contracts.Services
{
    public interface IMetadataService
    {
        Task<ProductsResponse> Products(ProductsRequest request);
    }
}
