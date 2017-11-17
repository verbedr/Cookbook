using Cookbook.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Cookbook.Repository.Configurations
{
    internal class HowToStepConfiguration : EntityTypeConfiguration<HowToStep>
    {
        public HowToStepConfiguration()
        {
            HasRequired(x => x.AppliesTo);
            HasOptional(x => x.NextStep);
        }
    }
}
