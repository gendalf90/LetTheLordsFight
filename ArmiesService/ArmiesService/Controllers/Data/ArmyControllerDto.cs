using System.ComponentModel.DataAnnotations;

namespace ArmiesService.Controllers.Data
{
    public class ArmyControllerDto
    {
        [Required(ErrorMessage = "Squads must be set")]
        public SquadContollerDto[] Squads { get; set; }
    }
}
