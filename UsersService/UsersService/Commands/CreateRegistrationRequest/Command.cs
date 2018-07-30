using System.Threading.Tasks;
using UsersDomain.Entities.Registration;
using UsersDomain.Repositories.Registration;
using UsersDomain.Services.Registration;
using UsersDomain.ValueTypes;
using UsersDomain.ValueTypes.Confirmation;
using UsersDomain.ValueTypes.Registration;
using UsersService.Common;
using UsersService.Logs;

namespace UsersService.Commands.CreateRegistrationRequest
{
    public class Command : ICommand
    {
        private readonly IGetConfirmationLinkStrategy confirmationLink;
        private readonly IRequests requestsRepository;
        private readonly IEmail emailService;
        private readonly ILog log;

        private RegistrationData registrationData;
        private Request registrationRequest;
        private Email registrationEmail;

        public Command(IGetConfirmationLinkStrategy confirmationLink, 
                       IRequests requestsRepository, 
                       IEmail emailService,
                       ILog log,
                       RegistrationData registrationData)
        {
            this.confirmationLink = confirmationLink;
            this.requestsRepository = requestsRepository;
            this.emailService = emailService;
            this.log = log;
            this.registrationData = registrationData;
        }

        public async Task ExecuteAsync()
        {
            LogStart();
            CreateRegistrationRequest();
            await SaveRegistrationRequestAsync();
            LogRequestSave();
            CreateRegistrationEmail();
            await SendRegistrationEmailAsync();
            LogEmailSend();
        }

        private void LogStart()
        {
            log.Information($"Start registration request creating for login: {registrationData.Login}");
        }

        private void CreateRegistrationRequest()
        {
            var login = new Login(registrationData.Login);
            var password = new Password(registrationData.Password);
            registrationRequest = Request.Create(login, password);
        }

        private async Task SaveRegistrationRequestAsync()
        {
            await registrationRequest.SaveAsync(requestsRepository);
        }

        private void LogRequestSave()
        {
            log.Information($"Registration request with id {registrationRequest.Id} has saved for login {registrationData.Login}");
        }

        private void CreateRegistrationEmail()
        {
            var url = confirmationLink.GetForRequestId(registrationRequest.Id);
            var link = new Link(url);
            registrationEmail = registrationRequest.CreateEmailWithConfirmationLink(link);
        }

        private async Task SendRegistrationEmailAsync()
        {
            await registrationEmail.SendAsync(emailService);
        }

        private void LogEmailSend()
        {
            log.Information($"Email for login {registrationData.Login} has sent");
        }
    }
}
