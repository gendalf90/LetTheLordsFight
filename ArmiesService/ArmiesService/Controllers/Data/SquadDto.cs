using System.ComponentModel.DataAnnotations;

namespace ArmiesService.Controllers.Data
{
    public class SquadDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Squad name must be set")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Quantity must be set")]
        public int Quantity { get; set; }

        public string[] Weapons { get; set; }

        public string[] Armors { get; set; }
    }
}
