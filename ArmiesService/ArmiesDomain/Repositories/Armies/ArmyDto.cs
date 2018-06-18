using System.Collections.Generic;

namespace ArmiesDomain.Repositories.Armies
{
    public class ArmyDto
    {
        public string OwnerLogin { get; set; }

        public List<SquadDto> Squads { get; set; }
    }
}
