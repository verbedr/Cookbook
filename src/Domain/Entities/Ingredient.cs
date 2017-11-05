using Common.Domain;
using System;
using System.ComponentModel.DataAnnotations;

namespace Cookbook.Domain.Entities
{
    public class Ingredient : Entity
    {
        protected Ingredient() { }

        public Ingredient(Recipe usedIn)
        {
            UsedIn = usedIn ?? throw new ArgumentNullException(nameof(usedIn));
        }

        [Required]
        public virtual Recipe UsedIn { get; private set; }
    }
}
