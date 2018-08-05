using ArmiesDomain.Exceptions;
using ArmiesDomain.Repositories.Armies;
using ArmiesDomain.Services.ArmyNotifications;

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
                throw new QuantityException();
            }

            this.value = value;
        }

        public Cost Multiply(Cost cost)
        {
            return cost.Multiply(value);
        }

        public void FillSquadData(SquadNotificationDto data)
        {
            data.Quantity = value;
        }

        public void FillSquadData(SquadRepositoryDto data)
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
