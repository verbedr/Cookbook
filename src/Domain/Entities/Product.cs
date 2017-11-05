using Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.Domain.Entities
{
    public class Product : Entity
    {
        protected Product() { }

        public Product(string name, Store from)
        {
            Name = name;
            From = from;
        }

        public string Name { get; private set; }

        public virtual Store From { get; private set; }
    }
}
