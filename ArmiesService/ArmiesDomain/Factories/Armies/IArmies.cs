using ArmiesDomain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArmiesDomain.Factories.Armies
{
    public interface IArmies
    {
        Army Build(ArmyData data);
    }
}
