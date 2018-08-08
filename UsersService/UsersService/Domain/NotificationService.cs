using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Tasks;
using UsersDomain.Services.Registration;

namespace UsersService.Domain
{
    class NotificationService : INotification
    {
        private const string EventKey = "events.user-created";
        private const string ExchangeName = "exchanges.events";

        private readonly IConnection connection;

        private IModel channel;

        public NotificationService(IConnection connection)
        {
            this.connection = connection;
        }

        public Task NotifyAsync(UserDto data)
        {
            InitializeChannel();
            InitializeExchange();
            SendEvent(data);
            return Task.CompletedTask;
        }

        private void InitializeChannel()
        {
            channel = connection.CreateModel();
        }

        private void InitializeExchange()
        {
            channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct);
        }

        private void SendEvent(UserDto data)
        {
            var json = JsonConvert.SerializeObject(data);
            var bytes = Encoding.UTF8.GetBytes(json);
            channel.BasicPublish(ExchangeName, EventKey, null, bytes);
        }
    }
}
