namespace Cookbook.Api.Models.RecipeModels
{
    public class OverviewModel
    {
        public HalLinkCollection _links { get; set; }

        public OverviewModelEmbedded _embedded { get; set; }

        public class OverviewModelEmbedded
        {
            public RecipeModel[] Item { get; set; }
        }
    }
}
