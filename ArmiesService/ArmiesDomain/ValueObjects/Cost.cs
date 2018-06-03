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
            this.value = value;
        }

        public Cost Add(Cost cost)
        {
            return new Cost(value + cost.value);
        }

        public bool IsGreaterThan(Cost cost)
        {
            return value > cost.value;
        }
    }
}
