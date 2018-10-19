using Cookbook.Contracts.Requests.RecipeRequests;
using Cookbook.Contracts.Responses.RecipeResponses;
using System.Threading.Tasks;

namespace Cookbook.Contracts.Services
{
    public interface IRecipeService
    {
        Task<OverviewResponse> Overview(OverviewRequest request);
        Task<LoadResponse> Load(LoadRequest request);

        Task<CreateResponse> Create(CreateRequest request);
        Task<AddIngredientResponse> AddIngredient(AddIngredientRequest request);
        Task<RemoveIngredientResponse> RemoveIngredient(RemoveIngredientRequest request);


    }
}
