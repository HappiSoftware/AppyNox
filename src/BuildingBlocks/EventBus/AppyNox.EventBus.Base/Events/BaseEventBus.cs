using AppyNox.EventBus.Base.Abstraction;
using AppyNox.EventBus.Base.SubscriptionManagers;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.EventBus.Base.Events
{
    public abstract class BaseEventBus : IEventBus
    {
        #region Fields

        public readonly IServiceProvider serviceProvider;

        public readonly IEventBusSubscriptionManager subsManager;

        public EventBusConfig eventBusConfig { get; set; }

        #endregion

        #region Public Constructors

        public BaseEventBus(EventBusConfig config, IServiceProvider serviceProvider)
        {
            eventBusConfig = config;
            this.serviceProvider = serviceProvider;
            subsManager = new InMemoryEventBusSubscriptionManager(ProcessEventName);
        }

        #endregion

        #region Public Methods

        public virtual string ProcessEventName(string eventName)
        {
            if (eventBusConfig.DeleteEventPrefix)
            {
                eventName = eventName.TrimStart(eventBusConfig.EventNamePrefix.ToArray());
            }
            if (eventBusConfig.DeleteEventSuffix)
            {
                eventName = eventName.TrimEnd(eventBusConfig.EventNameSuffix.ToArray());
            }

            return eventName;
        }

        public virtual string GetSubName(string eventName)
        {
            return $"{eventBusConfig.SubscriberClientAppName}.{ProcessEventName(eventName)}";
        }

        public virtual void Dispose()
        {
            eventBusConfig = null;
        }

        public async Task<bool> ProcessEvent(string eventName, string message)
        {
            eventName = ProcessEventName(eventName);

            var processed = false;

            if (subsManager.HasSubscriptionsForEvent(eventName))
            {
                var subscriptions = subsManager.GetHandlersForEvent(eventName);

                using (var scope = serviceProvider.CreateScope())
                {
                    foreach (var subscription in subscriptions)
                    {
                        var handler = serviceProvider.GetService(subscription.HandlerType);
                        if (handler == null)
                        {
                            continue;
                        }

                        var eventType = subsManager.GetEventTypeByName($"{eventBusConfig.EventNamePrefix}{eventName}{eventBusConfig.EventNameSuffix}");
                        if (eventType == null)
                        {
                            throw new ArgumentNullException($"Event type '{eventBusConfig.EventNamePrefix}{eventName}{eventBusConfig.EventNameSuffix}' not found!");
                        }
                        var integrationEvent = JsonConvert.DeserializeObject(message, eventType);

                        var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                        await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
                    }
                }
                processed = true;
            }
            return processed;
        }

        public abstract void Publish(IntegrationEvent @event);

        public abstract void Subscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>;

        public abstract void Unsubscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>;

        #endregion
    }
}