using ArmiesDomain.Repositories.Armies;
using ArmiesDomain.Repositories.Users;
using System;

namespace ArmiesDomain.ValueObjects
{
    public class Cost
    {
        private readonly int value;

        public Cost() : this(0)
        {
        }

        public Cost(int value)
        {
            if(value < 0)
            {
                throw new ArgumentException("Cost must be greater than or equal to 0");
            }

            this.value = value;
        }

        public Cost Add(Cost cost)
        {
            return new Cost(value + cost.value);
        }

        public Cost Multiply(int value)
        {
            return new Cost(this.value * value);
        }

        public bool IsGreaterThan(Cost cost)
        {
            return value > cost.value;
        }

        public void FillUserData(UserDto data)
        {
            data.ArmyCostLimit = value;
        }

        public void FillSquadData(SquadDto data)
        {
            data.Quantity = value;
        }
    }
}
