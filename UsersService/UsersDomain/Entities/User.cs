using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsersDomain.Repositories;
using UsersDomain.ValueTypes;

namespace UsersDomain.Entities
{
    public class User
    {
        private readonly Guid id;
        private readonly Login login;
        private readonly Password password;
        private readonly IEnumerable<Role> roles;

        private User(Guid id, Login login, Password password, IEnumerable<Role> roles)
        {
            this.id = id;
            this.login = login;
            this.password = password;
            this.roles = roles;
        }

        public Guid Id { get => id; }

        public async Task SaveAsync(IUsers repository)
        {
            var dto = CreateDtoToSave();
            await repository.SaveAsync(dto);
        }

        private UserDto CreateDtoToSave()
        {
            return new UserDto
            {
                Id = id,
                Login = login.ToString(),
                Password = password.ToString(),
                Roles = roles.ToArray()
            };
        }

        public static User CreateSimple(Login login, Password password)
        {
            return new User(Guid.NewGuid(),
                            login,
                            password,
                            new[] { Role.User });
        }
    }
}
