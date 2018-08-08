using ArmiesService.Commands;
using ArmiesService.Consumers.Data;
using ArmiesService.Logs;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArmiesService.Consumers
{
    class UserCreatedEventConsumer : IHostedService, IDisposable
    {
        private const string QueueName = "events.user-created.armies-service";
        private const string EventKey = "events.user-created";
        private const string ExchangeName = "exchanges.events";

        private readonly IConnection connection;
        private readonly ICommandsFactory commands;
        private readonly ILog logger;

        private IModel channel;

        public UserCreatedEventConsumer(IConnection connection, ICommandsFactory commands, ILog logger)
        {
            this.connection = connection;
            this.commands = commands;
            this.logger = logger;
        }

        public void Dispose()
        {
            channel?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            InitializeChannel();
            InitializeServiceQueue();
            InitializeErrorHandling();
            InitializeDataReceiving();
            return Task.CompletedTask;
        }

        private void InitializeChannel()
        {
            channel = connection.CreateModel();
        }

        private void InitializeServiceQueue()
        {
            channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct);
            channel.QueueDeclare(queue: QueueName, exclusive: false);
            channel.QueueBind(QueueName, ExchangeName, EventKey);
        }

        private void InitializeErrorHandling()
        {
            channel.CallbackException += (obj, args) =>
            {
                logger.Error(args.Exception, "An error while user creating");
            };
        }

        private void InitializeDataReceiving()
        {
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += async (obj, args) =>
            {
                var data = DeserializeUserData(args.Body);
                await CreateUserAsync(data);
            };
            channel.BasicConsume(QueueName, true, consumer);
        }

        private UserCreatedEventDto DeserializeUserData(byte[] bytes)
        {
            var json = Encoding.UTF8.GetString(bytes);
            return JsonConvert.DeserializeObject<UserCreatedEventDto>(json);
        }

        private async Task CreateUserAsync(UserCreatedEventDto data)
        {
            var command = commands.GetCreateUserCommand(data);
            await command.ExecuteAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            channel?.Close();
            return Task.CompletedTask;
        }
    }
}
