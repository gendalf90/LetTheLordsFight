using ArmiesDomain.Factories.Armies;
using ArmiesDomain.Services.Costs;
using ArmiesDomain.ValueObjects;
using System;
using System.Collections.Generic;

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

        public void Save(IArmies repository)
        {
            throw new NotImplementedException();
        }
    }
}
