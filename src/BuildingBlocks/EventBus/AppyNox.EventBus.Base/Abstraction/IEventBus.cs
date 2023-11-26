using AppyNox.EventBus.Base.Events;

namespace AppyNox.EventBus.Base.Abstraction
{
    public interface IEventBus
    {
        #region [ Public Methods ]

        void Publish(IntegrationEvent @event);

        void Subscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>;

        void Unsubscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>;

        #endregion
    }
}