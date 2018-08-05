using ArmiesDomain.Services.ArmyNotifications;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Tasks;

namespace ArmiesService.Domain.Services
{
    class ArmyNotificationsService : IArmyNotificationService
    {
        private const string EventKey = "events.army-created";
        private const string ExchangeName = "exchanges.events";

        private readonly IConnection connection;

        private IModel channel;

        public ArmyNotificationsService(IConnection connection)
        {
            this.connection = connection;
        }

        public Task NotifyThatCreatedAsync(ArmyNotificationDto data)
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

        private void SendEvent(ArmyNotificationDto data)
        {
            var json = JsonConvert.SerializeObject(data);
            var bytes = Encoding.UTF8.GetBytes(json);
            channel.BasicPublish(ExchangeName, EventKey, null, bytes);
        }
    }
}
