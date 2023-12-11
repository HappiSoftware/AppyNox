using AppyNox.EventBus.Base;
using AppyNox.EventBus.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;

namespace AppyNox.BuildingBlocks.EventBus.UnitTest.RabbitMQTests
{
    public class EventBusRabbitMQBaseTests
    {
        #region Fields

        private readonly ServiceProvider _sp;

        #endregion

        #region Public Constructors

        public EventBusRabbitMQBaseTests()
        {
            _sp = new ServiceCollection().BuildServiceProvider();
        }

        #endregion

        #region Public Methods

        [Fact]
        public void EventNameShouldBeReturnedAsBaseName()
        {
            EventBusConfig config = new()
            {
                EventNamePrefix = "Prefix",
                EventNameSuffix = "Suffix"
            };

            var bus = new EventBusRabbitMQ(config, _sp);

            var baseEventName = bus.ProcessEventName("PrefixTestBaseEventSuffix");

            Assert.Equal("TestBaseEvent", baseEventName);
        }

        [Fact]
        public void SubNameShouldReturnWithClientAppNameAndBaseEventName()
        {
            EventBusConfig config = new()
            {
                SubscriberClientAppName = "EventBus.UnitTests",
                EventNameSuffix = "IntegrationEvent"
            };

            var bus = new EventBusRabbitMQ(config, _sp);

            var subName = bus.GetSubName("TestCreatedIntegrationEvent");

            Assert.Equal("EventBus.UnitTests.TestCreated", subName);
        }

        #endregion
    }
}