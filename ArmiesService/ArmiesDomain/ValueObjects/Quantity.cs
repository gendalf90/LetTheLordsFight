using ArmiesDomain.Repositories.Armies;
using System;

namespace ArmiesDomain.ValueObjects
{
    public class Quantity
    {
        private readonly int value;

        public Quantity() : this(0)
        {
        }

        public Quantity(int value)
        {
            if(value < 0)
            {
                throw new ArgumentException("Quantity must be greater than or equal to 0");
            }

            this.value = value;
        }

        public Cost Multiply(Cost cost)
        {
            return cost.Multiply(value);
        }

        public void FillSquadData(SquadDto data)
        {
            data.Quantity = value;
        }
    }
}
