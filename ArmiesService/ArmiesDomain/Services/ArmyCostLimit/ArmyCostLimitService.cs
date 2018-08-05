using ArmiesDomain.Entities;
using ArmiesDomain.Exceptions;
using ArmiesDomain.ValueObjects;

namespace ArmiesDomain.Services
{
    public class ArmyCostLimitService : IArmyCostLimitService
    {
        private Cost currentCost;

        public ArmyCostLimitService()
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
                throw ArmyException.CreateCostLimitExceeded();
            }
        }
    }
}
