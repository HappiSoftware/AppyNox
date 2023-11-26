﻿using AppyNox.EventBus.Base.Events;

namespace AppyNox.EventBus.Base.Abstraction
{
    public interface IIntegrationEventHandler<TIntegrationEvent> : IntegrationEventHandler where TIntegrationEvent : IntegrationEvent
    {
        #region [ Public Methods ]

        Task Handle(TIntegrationEvent @event);

        #endregion
    }

    public interface IntegrationEventHandler
    {
    }
}