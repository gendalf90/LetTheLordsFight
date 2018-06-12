using System.ComponentModel.DataAnnotations;

namespace ArmiesService.Controllers.Data
{
    public class ArmyDto
    {
        [Required(ErrorMessage = "Squads must be set")]
        public SquadDto[] Squads { get; set; }
    }
}
