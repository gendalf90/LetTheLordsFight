using ArmiesDomain.Exceptions;
using ArmiesDomain.Repositories.Armies;

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
                throw new QuantityException("Quantity must be greater than or equal to 0");
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

        public bool IsZero
        {
            get => value == 0;
        }

        public static readonly Quantity Single = new Quantity(1);
    }
}
