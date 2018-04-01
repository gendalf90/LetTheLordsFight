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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            CreateUserModel(modelBuilder);
            CreateRoleModel(modelBuilder);
            CreateRegistrationRequestModel(modelBuilder);
        }

        private void CreateUserModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserDto>().ToTable("users");
            modelBuilder.Entity<UserDto>().HasKey(user => user.Id).HasName("id");
            modelBuilder.Entity<UserDto>().Property(user => user.Login).HasColumnName("login");
            modelBuilder.Entity<UserDto>().Property(user => user.Password).HasColumnName("password");
        }

        private void CreateRoleModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoleDto>().ToTable("roles");
            modelBuilder.Entity<RoleDto>().Property(role => role.UserId).HasColumnName("userid");
            modelBuilder.Entity<RoleDto>().Property(role => role.Value).HasColumnName("role");
            modelBuilder.Entity<RoleDto>().HasKey(role => new { role.UserId, role.Value });
        }

        private void CreateRegistrationRequestModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RegistrationRequestDto>().ToTable("registration_requests");
            modelBuilder.Entity<RegistrationRequestDto>().HasKey(request => request.Id).HasName("id");
            modelBuilder.Entity<RegistrationRequestDto>().Property(request => request.Login).HasColumnName("login");
            modelBuilder.Entity<RegistrationRequestDto>().Property(request => request.Password).HasColumnName("password");
            modelBuilder.Entity<RegistrationRequestDto>().Property(request => request.TTLSeconds).HasColumnName("ttl_seconds");
        }
    }

}
