using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
    public class Entity : IEntity
    {
        protected Entity()
        {
            Id = 0;
        }

        public int Id { get; private set; }

        public virtual bool IsNew()
        {
            return Id == 0;
        }

        public class IdComparer<T> : IEqualityComparer<T>
            where T : Entity
        {
            public bool Equals(T x, T y)
            {
                return x != null && y != null && Equals(x.Id, y.Id);
            }

            public int GetHashCode(T obj)
            {
                return obj.Id.GetHashCode();
            }
        }
    }
}
