using Common.Domain;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Cookbook.Domain.Entities
{
    public class Recipe : Entity
    {
        protected Recipe() { }

        public string Uri { get; set; }

        public string Name { get; set; }

        public int NumberOfPlates { get; private set; }

        #region Ingredients
        public ReadOnlyCollection<Ingredient> Ingredients => _Ingredients.ToList().AsReadOnly();

        protected internal virtual ICollection<Ingredient> _Ingredients { get; private set; } = new List<Ingredient>();
        #endregion

        public virtual HowToStep Start { get; set; }

    }
}
