using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using UsersDomain.Services.Registration;
using UsersService.Options;

namespace UsersService.Domain
{
    public class EmailService : IEmail
    {
        private readonly IOptions<SmtpOptions> options;

        public EmailService(IOptions<SmtpOptions> options)
        {
            this.options = options;
        }

        public async Task SendAsync(EmailDto data)
        {
            var client = new SmtpClient(options.Value.Host, options.Value.Port)
            {
                Credentials = new NetworkCredential(options.Value.Login, options.Value.Password),
                EnableSsl = true
            };
            await client.SendMailAsync(options.Value.Login, data.Address, data.Head, data.Body);
        }
    }
}
