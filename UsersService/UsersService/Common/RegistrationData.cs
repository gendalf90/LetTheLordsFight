using System.ComponentModel.DataAnnotations;

namespace UsersService.Common
{
    public class RegistrationData
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
