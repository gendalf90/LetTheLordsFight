using ArmiesDomain.Entities;
using ArmiesDomain.ValueObjects;

namespace ArmiesDomain.Services
{
    public interface IArmyCostLimitService
    {
        void AccumulateCost(Cost cost);

        void CheckForUser(User user);
    }
}
