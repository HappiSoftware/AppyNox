using AppyNox.EventBus.Base;
using AppyNox.EventBus.Base.Abstraction;
using AppyNox.EventBus.RabbitMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.EventBus.Factory
{
    public static class EventBusFactory
    {
        #region Public Methods

        public static IEventBus Create(EventBusConfig config, IServiceProvider serviceProvider)
        {
            // switch case : config.EventBusType ? AzureServiceBus OR RabbitMQ
            return new EventBusRabbitMQ(config, serviceProvider);
        }

        #endregion
    }
}