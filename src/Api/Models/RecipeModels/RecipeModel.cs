using Cookbook.Contracts.PropertyBags;

namespace Cookbook.Api.Models.RecipeModels
{
    public class RecipeModel : Recipe
    {
        public HalLinkCollection _links { get; set; }
    }
}
