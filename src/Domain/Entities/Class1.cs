using Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.Domain.Entities
{
    public class UserProfile : Entity
    {

        public virtual UserProfile Organizer { get; private set; }

        public FoodPreference Peferences { get; private set; }
    }

    [Flags]
    public enum FoodPreference
    {
        All = 0,
        NoGluten = 1,
        NoFish = 2,
        
    }

    public class ProductSupply
    {
        public virtual Product Product { get; private set; }

        public int Amount { get; private set; }

        public virtual UserProfile HomeOf { get; private set; }

        public DateTime BestBefore { get; private set; }
    }

    public class WeekPlan
    {
        public virtual UserProfile HomeOf { get; private set; }


        public virtual ICollection<DaySelection> Selections { get; private set; }
    }

    public class DaySelection
    {
        public virtual ICollection<UserProfile> For { get; private set; }

        public virtual Recipe Recipe { get; private set; }
    }
}
