using Microsoft.EntityFrameworkCore;

namespace UsersService.Database
{
    class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }

        public DbSet<UserDto> Users { get; set; }

        public DbSet<RegistrationRequestDto> RegistrationRequests { get; set; }
    }

}
