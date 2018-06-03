using System.ComponentModel.DataAnnotations;

namespace ArmiesService.Controllers.Data
{
    public class ArmyDto
    {
        [Required(ErrorMessage = "Squads must be set")]
        [MinLength(1, ErrorMessage = "No squads")]
        public SquadDto[] Squads { get; set; }
    }
}
