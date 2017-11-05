using Common.Domain;
using Cookbook.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.Domain.Entities
{
    public class HowToStep : Entity
    {
        protected HowToStep() { }

        public HowToStep(Recipe appliesTo)
        {
            AppliesTo = appliesTo ?? throw new ArgumentNullException(nameof(appliesTo));
        }

        public void AddNextStep(HowToStep nextStep)
        {
            this.Verify(x => x.NextStep, nextStep)
                .Verify(x=>x.AppliesTo.Id == nextStep.AppliesTo.Id, "ShouldBelongToSameRecipe")
                .ThrowIfInvalid();

            NextStep = nextStep;
        }

        public Recipe AppliesTo { get; private set; }

        public HowToStep NextStep { get; private set; }
    }
}
