using System.Collections.Generic;

namespace ArmiesDomain.Factories.Armies
{
    public class ArmyData
    {
        public string OwnerLogin { get; set; }

        public List<SquadData> Squads { get; set; }
    }
}
