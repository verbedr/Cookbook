using Common.Domain;
using System.ComponentModel.DataAnnotations;

namespace Cookbook.Domain.Entities
{
    public class ProductAsIngredient : Entity
    {
        protected ProductAsIngredient() { }

        internal ProductAsIngredient(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }

        [Required]
        public virtual Product Product { get; private set; }

        [Required]
        public int Quantity { get; private set; }
    }
}
