using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsersDomain.Repositories;
using UsersDomain.Services.Registration;
using UsersDomain.ValueTypes;
using UserRepositoryDto = UsersDomain.Repositories.UserDto;
using UserNotificationDto = UsersDomain.Services.Registration.UserDto;

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

        public Guid Id { get; private set; }

        public async Task SaveAsync(IUsers repository)
        {
            var dto = CreateDtoToSave();
            await repository.SaveAsync(dto);
        }

        public async Task NotifyThatRegisteredAsync(INotification service)
        {
            var dto = CreateDtoToNotify();
            await service.NotifyAsync(dto);
        }

        private UserNotificationDto CreateDtoToNotify()
        {
            return new UserNotificationDto
            {
                Login = login.ToString()
            };
        }

        private UserRepositoryDto CreateDtoToSave()
        {
            return new UserRepositoryDto
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
