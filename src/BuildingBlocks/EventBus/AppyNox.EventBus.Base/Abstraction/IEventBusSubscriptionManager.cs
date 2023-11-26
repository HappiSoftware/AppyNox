using AppyNox.EventBus.Base.Events;

namespace AppyNox.EventBus.Base.Abstraction
{
    public interface IEventBusSubscriptionManager
    {
        #region [ Events ]

        event EventHandler<string> OnEventRemoved;

        #endregion Events

        #region [ Properties ]

        bool IsEmpty { get; }

        #endregion Properties

        #region [ Public Methods ]

        void AddSubscription<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>;

        void RemoveSubscription<T, TH>() where TH : IIntegrationEventHandler<T> where T : IntegrationEvent;

        bool HasSubscriptionsForEvent<T>() where T : IntegrationEvent;

        bool HasSubscriptionsForEvent(string eventName);

        Type? GetEventTypeByName(string eventName);

        void Clear();

        IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : IntegrationEvent;

        IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName);

        string GetEventKey<T>();

        #endregion Public Methods
    }
}