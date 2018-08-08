using System.ComponentModel.DataAnnotations;

namespace ArmiesService.Controllers.Data
{
    public class ArmyPostDto
    {
        [Required(ErrorMessage = "Squads must be set")]
        public SquadPostDto[] Squads { get; set; }
    }
}
