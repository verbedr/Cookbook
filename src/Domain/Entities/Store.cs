using Common.Domain;

namespace Cookbook.Domain.Entities
{
    public class Store : Entity
    {
        protected Store() { }

        public Store(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}
