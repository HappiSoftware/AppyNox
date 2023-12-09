using AppyNox.EventBus.Base;
using AppyNox.EventBus.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;

namespace AppyNox.BuildingBlocks.EventBus.UnitTest.RabbitmqTests
{
    public class EventBusRabbitmqBaseTests
    {
        #region Fields

        private ServiceProvider _sp;

        #endregion

        #region Public Constructors

        public EventBusRabbitmqBaseTests()
        {
            ServiceCollection _services = new ServiceCollection();
            _sp = _services.BuildServiceProvider();
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