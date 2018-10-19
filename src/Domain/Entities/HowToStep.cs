using Common.Domain;
using Cookbook.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.Domain.Entities
{
    public class HowToStep : Entity
    {
        private string _description;

        protected HowToStep() { }

        public HowToStep(Recipe appliesTo, string description)
        {
            AppliesTo = appliesTo ?? throw new ArgumentNullException(nameof(appliesTo));
            Description = description;
        }

        public void AddNextStep(HowToStep nextStep)
        {
            this.Verify(x => x.NextStep, nextStep)
                .Verify(x => x.AppliesTo.Id == nextStep.AppliesTo.Id, "ShouldBelongToSameRecipe")
                .ThrowIfInvalid();

            NextStep = nextStep;
        }

        public Recipe AppliesTo { get; private set; }

        public HowToStep NextStep { get; private set; }

        [StringLength(1000)]
        public string Description
        {
            get => _description; set
            {
                this.Verify(x => x.Description, value).ThrowIfInvalid();
                _description = value;
            }
        }

        #region Alternatives
        public ReadOnlyCollection<HowToStep> Alternatives => _Alternatives.ToList().AsReadOnly();

        protected internal virtual ICollection<HowToStep> _Alternatives { get; private set; } = new HashSet<HowToStep>();
        #endregion

        #region Needs
        public ReadOnlyCollection<Ingredient> Needs => _Needs.ToList().AsReadOnly();

        protected internal virtual ICollection<Ingredient> _Needs { get; private set; } = new HashSet<Ingredient>();
        #endregion
    }
}
