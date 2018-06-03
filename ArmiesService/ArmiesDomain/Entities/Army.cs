using ArmiesDomain.Factories.Armies;
using ArmiesDomain.Services.Costs;
using ArmiesDomain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArmiesDomain.Entities
{
    public class Army
    {
        public Army(IEnumerable<Squad> squads)
        {

        }

        public void Apply(ICost cost)
        {
            throw new NotImplementedException();
        }

        public async Task SaveAsync(IArmies repository)
        {
            throw new NotImplementedException();
        }
    }
}
