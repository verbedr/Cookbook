using Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.Domain.Entities
{
    public class Recipe : Entity
    {
        protected Recipe() { }

        public string Uri { get; set; }

        public string Name { get; set; }

        #region Ingredients
        public Ingredient[] Ingredients => _Ingredients.ToArray();

        protected internal virtual ICollection<Ingredient> _Ingredients { get; private set; } = new List<Ingredient>();
        #endregion

        public virtual HowToStep Start { get; set; }


    }
}
