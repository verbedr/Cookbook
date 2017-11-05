using Cookbook.Contracts.Requests.RecipeRequests;
using Cookbook.Contracts.Requests.RecipeResponses;
using System.Threading.Tasks;

namespace Cookbook.Contracts.Services
{
    public interface IRecipeService
    {
        Task<OverviewResponse> Overview(OverviewRequest request);
    }
}
