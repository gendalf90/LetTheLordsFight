using ArmiesDomain.Entities;
using ArmiesDomain.ValueObjects;

namespace ArmiesDomain.Services
{
    public interface IArmyCostLimit
    {
        void AccumulateCost(Cost cost);

        void CheckForUser(User user);
    }
}
