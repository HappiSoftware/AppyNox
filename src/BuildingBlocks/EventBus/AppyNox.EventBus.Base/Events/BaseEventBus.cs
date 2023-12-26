using AppyNox.EventBus.Base.Abstraction;
using AppyNox.EventBus.Base.SubscriptionManagers;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AppyNox.EventBus.Base.Events
{
    public abstract class BaseEventBus : IEventBus
    {
        #region [ Fields ]

        public readonly IServiceProvider serviceProvider;

        public readonly IEventBusSubscriptionManager subsManager;

        protected static readonly JsonSerializerOptions jsonSerializerOptions = new()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };

        #endregion

        #region [ Properties ]

        public EventBusConfig EventBusConfig { get; set; }

        #endregion

        #region [ Public Constructors ]

        protected BaseEventBus(EventBusConfig config, IServiceProvider serviceProvider)
        {
            EventBusConfig = config;
            this.serviceProvider = serviceProvider;
            subsManager = new InMemoryEventBusSubscriptionManager(ProcessEventName);
        }

        #endregion

        #region [ Public Methods ]

        public virtual string ProcessEventName(string eventName)
        {
            if (EventBusConfig.DeleteEventPrefix)
            {
                eventName = eventName.TrimStart(EventBusConfig.EventNamePrefix.ToArray());
            }
            if (EventBusConfig.DeleteEventSuffix)
            {
                eventName = eventName.TrimEnd(EventBusConfig.EventNameSuffix.ToArray());
            }

            return eventName;
        }

        public virtual string GetSubName(string eventName)
        {
            return $"{EventBusConfig.SubscriberClientAppName}.{ProcessEventName(eventName)}";
        }

        public virtual void Dispose()
        {
            Dispose(true);
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

                        var eventType = subsManager.GetEventTypeByName($"{EventBusConfig.EventNamePrefix}{eventName}{EventBusConfig.EventNameSuffix}");
                        if (eventType == null)
                        {
                            throw new ArgumentNullException($"Event type '{EventBusConfig.EventNamePrefix}{eventName}{EventBusConfig.EventNameSuffix}' not found!");
                        }
                        var integrationEvent = JsonConvert.DeserializeObject(message, eventType);

                        var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                        await (Task)concreteType.GetMethod("Handle")!.Invoke(handler!, [integrationEvent])!;
                    }
                }
                processed = true;
            }
            return processed;
        }

        public abstract void Publish(IntegrationEvent @event);

        public abstract void Subscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>;

        public abstract void Unsubscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>;

        protected virtual void Dispose(bool disposing)
        {
            EventBusConfig = null!;
        }

        #endregion
    }
}