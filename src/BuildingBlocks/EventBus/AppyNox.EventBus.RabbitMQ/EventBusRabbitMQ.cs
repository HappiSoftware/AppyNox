using AppyNox.EventBus.Base;
using AppyNox.EventBus.Base.Events;
using Newtonsoft.Json;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.EventBus.RabbitMQ
{
    public class EventBusRabbitMQ : BaseEventBus
    {
        #region Fields

        private readonly IConnectionFactory _connectionFactory;

        private readonly IModel _consumerChannel;

        private RabbitMQPersistentConnection persistentConnection;

        #endregion

        #region Public Constructors

        public EventBusRabbitMQ(EventBusConfig config, IServiceProvider serviceProvider) : base(config, serviceProvider)
        {
            if (config.Connection != null)
            {
                var connJson = JsonConvert.SerializeObject(eventBusConfig.Connection, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

                _connectionFactory = JsonConvert.DeserializeObject<ConnectionFactory>(connJson);
            }
            else
            {
                _connectionFactory = new ConnectionFactory();
            }

            persistentConnection = new RabbitMQPersistentConnection(_connectionFactory, config.ConnectionRetryCount);

            _consumerChannel = CreateConsumerChannel();

            subsManager.OnEventRemoved += SubsManager_OnEventRemoved;
        }

        private void SubsManager_OnEventRemoved(object? sender, string eventName)
        {
            eventName = ProcessEventName(eventName);

            if (!persistentConnection.IsConnected)
            {
                persistentConnection.TryConnect();
            }

            _consumerChannel.QueueUnbind(queue: eventName,
                        exchange: eventBusConfig.DefaultTopicName,
                        routingKey: eventName);

            if (subsManager.IsEmpty)
            {
                _consumerChannel.Close();
            }
        }

        #endregion

        #region Public Methods

        public override void Publish(IntegrationEvent @event)
        {
            if (!persistentConnection.IsConnected)
            {
                persistentConnection.TryConnect();
            }

            var policy = Policy.Handle<BrokerUnreachableException>()
            .Or<SocketException>()
            .WaitAndRetry(eventBusConfig.ConnectionRetryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
            {
                // log
            });

            var eventName = @event.GetType().Name;
            eventName = ProcessEventName(eventName);
            _consumerChannel.ExchangeDeclare(exchange: eventBusConfig.DefaultTopicName, type: "direct"); // Ensure exchange exists while publishing
            var message = JsonConvert.SerializeObject(@event);
            var body = Encoding.UTF8.GetBytes(message);
            policy.Execute(() =>
            {
                var properties = _consumerChannel.CreateBasicProperties();
                properties.DeliveryMode = 2; // persistent

                _consumerChannel.QueueDeclare(queue: GetSubName(eventName), // Ensure queue exists while publishing
                                durable: true,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);

                _consumerChannel.BasicPublish(exchange: eventBusConfig.DefaultTopicName,
                                routingKey: eventName,
                                mandatory: true,
                                basicProperties: properties, body: body);
            });
        }

        public override void Subscribe<T, TH>()
        {
            var eventName = typeof(T).Name;
            eventName = ProcessEventName(eventName);

            if (!subsManager.HasSubscriptionsForEvent(eventName))
            {
                if (!persistentConnection.IsConnected)
                {
                    persistentConnection.TryConnect();
                }

                _consumerChannel.QueueDeclare(queue: GetSubName(eventName),
                                durable: true,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);

                _consumerChannel.QueueBind(queue: GetSubName(eventName),
                                exchange: eventBusConfig.DefaultTopicName,
                                routingKey: eventName);
            }

            subsManager.AddSubscription<T, TH>();
            StartBasicConsume(eventName);
        }

        public override void Unsubscribe<T, TH>()
        {
            subsManager.RemoveSubscription<T, TH>();
        }

        private IModel CreateConsumerChannel()
        {
            if (!persistentConnection.IsConnected)
            {
                persistentConnection.TryConnect();
            }

            var channel = persistentConnection.CreateModel();

            channel.ExchangeDeclare(exchange: eventBusConfig.DefaultTopicName, type: "direct");

            return channel;
        }

        private void StartBasicConsume(string eventName)
        {
            if (_consumerChannel != null)
            {
                var consumer = new EventingBasicConsumer(_consumerChannel);

                consumer.Received += Consumer_Received;

                _consumerChannel.BasicConsume(queue: GetSubName(eventName),
                                autoAck: false,
                                consumer: consumer);
            }
        }

        private async void Consumer_Received(object? sender, BasicDeliverEventArgs eventArgs)
        {
            var eventName = eventArgs.RoutingKey;
            eventName = ProcessEventName(eventName);
            var message = Encoding.UTF8.GetString(eventArgs.Body.Span);

            try
            {
                await ProcessEvent(eventName, message);
            }
            catch (Exception e)
            {
                // Logging
            }

            _consumerChannel.BasicAck(eventArgs.DeliveryTag, multiple: false);
        }

        #endregion
    }
}