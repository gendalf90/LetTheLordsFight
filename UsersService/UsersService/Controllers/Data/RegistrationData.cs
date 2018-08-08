using System.ComponentModel.DataAnnotations;

namespace UsersService.Controllers.Data
{
    public class RegistrationData
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
