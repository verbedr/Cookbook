using Common.Domain;
using Cookbook.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Cookbook.Domain.Entities
{
    public class Ingredient : Entity
    {
        protected Ingredient() { }

        public Ingredient(Recipe usedIn, string commonName)
        {
            UsedIn = usedIn ?? throw new ArgumentNullException(nameof(usedIn));
            CommonName = commonName;
        }

        [Required]
        public virtual Recipe UsedIn { get; private set; }

        public string CommonName { get; private set; }

        public int Quantity { get; set; }

        public CookingUnit Unit { get; set; }

        #region PossibleProducts
        public ReadOnlyCollection<ProductAsIngredient> PossibleProducts => _PossibleProducts.ToList().AsReadOnly();

        protected internal virtual ICollection<ProductAsIngredient> _PossibleProducts { get; private set; } = new List<ProductAsIngredient>();
        #endregion

        public void AddProductChoice(Product product, int quantity)
        {
            this.Verify(x => !_PossibleProducts.Any(p => p.Product == product), "CanNotAddProductTwice")
                .ThrowIfInvalid();
            _PossibleProducts.Add(new ProductAsIngredient(product, quantity));
        }
    }
}
