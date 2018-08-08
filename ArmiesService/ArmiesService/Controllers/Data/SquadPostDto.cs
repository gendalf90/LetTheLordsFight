using System.ComponentModel.DataAnnotations;

namespace ArmiesService.Controllers.Data
{
    public class SquadPostDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Squad type must be set")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Quantity must be set")]
        public int? Quantity { get; set; }

        public string[] Weapons { get; set; }

        public string[] Armors { get; set; }
    }
}
