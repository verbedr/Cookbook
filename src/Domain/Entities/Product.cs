using Common.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.Domain.Entities
{
    public class Product : Entity
    {
        protected Product() { }

        public Product(string name, string brand, string reference)
        {
            Name = name;
            Brand = brand;
            Reference = reference;
        }

        public string Name { get; private set; }
        public string Brand { get; private set; }
        public string Reference { get; private set; }

        #region SoldAt
        public ReadOnlyCollection<Store> SoldAt => _SoldAt.ToList().AsReadOnly();

        protected internal virtual ICollection<Store> _SoldAt { get; private set; } = new HashSet<Store>();
        #endregion

        public virtual void FoundIn(Store store)
        {
            _SoldAt.Add(store);
        }

        public virtual void RemovedFrom(Store store)
        {
            _SoldAt.Remove(store);
        }
    }
}
