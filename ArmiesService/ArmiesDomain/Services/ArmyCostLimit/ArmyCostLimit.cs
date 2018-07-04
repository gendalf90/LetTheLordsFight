using ArmiesDomain.Entities;
using ArmiesDomain.Exceptions;
using ArmiesDomain.ValueObjects;

namespace ArmiesDomain.Services
{
    public class ArmyCostLimit : IArmyCostLimit
    {
        private Cost currentCost;

        public ArmyCostLimit()
        {
            currentCost = new Cost();
        }

        public void AccumulateCost(Cost cost)
        {
            currentCost = currentCost.Add(cost);
        }

        public void CheckForUser(User user)
        {
            if (user.IsArmyCostLimitExceeded(currentCost))
            {
                throw new ArmyException("The army is too expensive", "cost");
            }
        }
    }
}
