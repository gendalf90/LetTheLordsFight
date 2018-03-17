using System;
using System.Threading.Tasks;
using UsersDomain.Repositories.Registration;
using UsersDomain.ValueTypes;
using UsersDomain.ValueTypes.Confirmation;
using UsersDomain.ValueTypes.Registration;

namespace UsersDomain.Entities.Registration
{
    public class Request
    {
        private readonly Guid id;
        private readonly Login login;
        private readonly Password password;
        private readonly TTL ttl;

        private Request(Guid id, Login login, Password password, TTL ttl)
        {
            this.login = login;
            this.password = password;
            this.ttl = ttl;
            this.id = id;
        }

        public Guid Id { get => id; }

        public async Task SaveAsync(IRequests repository)
        {
            var dto = CreateDtoToSave();
            await repository.SaveAsync(dto);
        }

        private RequestDto CreateDtoToSave()
        {
            return new RequestDto
            {
                Id = Id,
                Login = login.ToString(),
                Password = password.ToString(),
                TTL = ttl.ToTimeSpan()
            };
        }

        public static Request Create(Login login, Password password)
        {
            return new Request(Guid.NewGuid(),
                               login,
                               password,
                               TTL.CreateDefault());
        }

        public static async Task<Request> LoadAsync(Guid id, IRequests repository)
        {
            var dto = await repository.GetByIdAsync(id);
            return new Request(dto.Id,
                               new Login(dto.Login),
                               new Password(dto.Password),
                               new TTL(dto.TTL));
        }

        public User CreateSimpleUser()
        {
            return User.CreateSimple(login, password);
        }

        public Email CreateEmailWithConfirmationLink(Link link)
        {
            return new Email(login, password, link, ttl);
        }
    }
}
