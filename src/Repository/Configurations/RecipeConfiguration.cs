using Cookbook.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Cookbook.Repository.Configurations
{
    internal class RecipeConfiguration : EntityTypeConfiguration<Recipe>
    {
        public RecipeConfiguration()
        {
            Ignore(x => x.Ingredients);
            HasMany(x => x._Ingredients).WithRequired(x => x.UsedIn);
        }
    }
}
