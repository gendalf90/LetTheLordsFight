using System.Collections.Generic;

namespace ArmiesDomain.Factories.Armies
{
    public class ArmyFactoryDto
    {
        public string OwnerLogin { get; set; }

        public List<SquadFactoryDto> Squads { get; set; }
    }
}
