using AppyNox.EventBus.Base;
using AppyNox.EventBus.Base.Abstraction;
using AppyNox.EventBus.RabbitMQ;

namespace AppyNox.EventBus.Factory
{
    public static class EventBusFactory
    {
        #region [ Public Methods ]

        public static IEventBus Create(EventBusConfig config, IServiceProvider serviceProvider)
        {
            // switch case : config.EventBusType ? AzureServiceBus OR RabbitMQ
            return new EventBusRabbitMQ(config, serviceProvider);
        }

        #endregion
    }
}