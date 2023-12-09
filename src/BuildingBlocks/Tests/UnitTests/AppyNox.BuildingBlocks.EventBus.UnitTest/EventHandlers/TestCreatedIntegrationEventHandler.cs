using AppyNox.BuildingBlocks.EventBus.UnitTest.Events;
using AppyNox.EventBus.Base.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.BuildingBlocks.EventBus.UnitTest.EventHandlers
{
    public class TestCreatedIntegrationEventHandler : IIntegrationEventHandler<TestCreatedIntegrationEvent>
    {
        #region Public Methods

        public Task Handle(TestCreatedIntegrationEvent @event)
        {
            return Task.CompletedTask;
        }

        #endregion
    }
}