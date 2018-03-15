using System.Threading.Tasks;
using UsersDomain.Services.Registration;
using UsersDomain.ValueTypes.Confirmation;

namespace UsersDomain.ValueTypes.Registration
{
    public class Email
    {
        private readonly Login login;
        private readonly Password password;
        private readonly Link confirmationLink;
        private readonly TTL requestTTL;

        public Email(Login login, Password password, Link confirmationLink, TTL requestTTL)
        {
            this.login = login;
            this.password = password;
            this.confirmationLink = confirmationLink;
            this.requestTTL = requestTTL;
        }

        public async Task SendAsync(IEmail service)
        {
            var dto = CreateDto();
            await service.SendAsync(dto);
            
            //body in dto: (login + password + request confirm PAGE path)
        }

        private EmailDto CreateDto()
        {
            return new EmailDto
            {
                Address = GetAddress(),
                Head = GetHead(),
                Body = GetBody()
            };
        }

        private string GetAddress()
        {
            return login.ToString();
        }

        private string GetHead()
        {
            return "test";
        }

        private string GetBody()
        {
            return "test";
        }
    }
}
