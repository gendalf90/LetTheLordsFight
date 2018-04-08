using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;
using UsersDomain.Services.Registration;
using UsersService.Options;
using MimeKit.Text;

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
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(options.Value.Login));
            message.To.Add(new MailboxAddress(data.Address));
            message.Subject = data.Head;
            message.Body = new TextPart(TextFormat.Html)
            {
                Text = data.Body
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(options.Value.Host, options.Value.Port);
                await client.AuthenticateAsync(options.Value.Login, options.Value.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}
