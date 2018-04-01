using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace UsersService.Database
{
    class RegistrationRequestDto
    {
        public Guid Id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public double TTLSeconds { get; set; }
    }
}
