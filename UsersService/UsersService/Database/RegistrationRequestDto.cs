using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace UsersService.Database
{
    [Table("registration_requests")]
    class RegistrationRequestDto
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Column("login")]
        public string Login { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("ttl_seconds")]
        public double TTLSeconds { get; set; }

        [Column("create_date")]
        public DateTime CreateDate { get; set; }
    }
}
