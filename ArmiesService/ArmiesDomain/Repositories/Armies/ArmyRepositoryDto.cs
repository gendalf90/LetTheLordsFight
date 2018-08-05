using System.Collections.Generic;

namespace ArmiesDomain.Repositories.Armies
{
    public class ArmyRepositoryDto
    {
        public string OwnerLogin { get; set; }

        public List<SquadRepositoryDto> Squads { get; set; }
    }
}
