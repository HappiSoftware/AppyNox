using AppyNox.BuildingBlocks.EventBus.UnitTest.EventHandlers;
using AppyNox.BuildingBlocks.EventBus.UnitTest.Events;
using AppyNox.EventBus.Base;
using AppyNox.EventBus.Base.Abstraction;
using AppyNox.EventBus.Factory;
using Microsoft.Extensions.DependencyInjection;

namespace AppyNox.BuildingBlocks.EventBus.UnitTest.RabbitmqTests
{
    public class EventBusRabbitmqTests
    {
        #region Fields

        private IEventBus _eventBus;

        #endregion

        #region Public Constructors

        public EventBusRabbitmqTests()
        {
            ServiceCollection _services = new ServiceCollection();
            _services.AddSingleton(sp =>
            {
                EventBusConfig config = new()
                {
                    ConnectionRetryCount = 1,
                    SubscriberClientAppName = "AppyNox.EventBus.UnitTest",
                    EventBusType = EventBusType.RabbitMQ
                };

                return EventBusFactory.Create(config, sp);
            });

            var sp = _services.BuildServiceProvider();

            _eventBus = sp.GetRequiredService<IEventBus>();
        }

        #endregion

        #region Public Methods

        [Fact]
        public void ShouldSendMessage()
        {
            _eventBus.Publish(new TestCreatedIntegrationEvent(1));
        }

        [Fact]
        public void ShouldSubscribeToEvent()
        {
            _eventBus.Subscribe<TestCreatedIntegrationEvent, TestCreatedIntegrationEventHandler>();
        }

        #endregion
    }
}