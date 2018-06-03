using ArmiesDomain.Entities;
using ArmiesDomain.ValueObjects;

namespace ArmiesDomain.Services.Costs
{
    public class ArmyLimitForUser : ICost
    {
        private User currentUser;
        private Cost currentCost;

        public ArmyLimitForUser(User user)
        {
            currentUser = user;
            currentCost = new Cost();
        }

        public void Add(Cost cost)
        {
            currentCost = currentCost.Add(cost);

            if(currentUser.IsArmyCostLimitExceeded(currentCost))
            {
                //throw
            }
        }
    }
}
