using System;
using System.Threading.Tasks;
using UsersDomain.Services.Registration;

namespace UsersService.Domain
{
    public class EmailService : IEmail
    {
        public Task SendAsync(EmailDto data)
        {
            throw new NotImplementedException();
        }
    }
}
