using AppyNox.BuildingBlocks.EventBus.UnitTest.Events;
using AppyNox.EventBus.Base.Abstraction;
using AppyNox.EventBus.Base.SubscriptionManagers;

namespace AppyNox.BuildingBlocks.EventBus.UnitTest.SubscriptionManagerTests
{
    public class InMemoryEventBusSubscriptionManagerTests
    {
        #region Public Methods

        [Fact]
        public void GetEventKey_ShouldReturnFormatted()
        {
            // Arrange
            var subscriptionManager = new InMemoryEventBusSubscriptionManager(e => e.TrimEnd("IntegrationEvent".ToArray()));
            var eventName = "TestCreated";

            // Act
            var eventKey = subscriptionManager.GetEventKey<TestCreatedIntegrationEvent>();

            // Assert
            Assert.Equal(eventName, eventKey);
        }

        [Fact]
        public void AddSubscription_ShouldAddHandler()
        {
            // Arrange
            var subscriptionManager = new InMemoryEventBusSubscriptionManager(e => e);
            var eventName = "TestCreatedIntegrationEvent";

            // Act
            subscriptionManager.AddSubscription<TestCreatedIntegrationEvent, IIntegrationEventHandler<TestCreatedIntegrationEvent>>();

            // Assert
            Assert.True(subscriptionManager.HasSubscriptionsForEvent(eventName));
        }

        [Fact]
        public void RemoveSubscription_ShouldRemoveHandler()
        {
            // Arrange
            var subscriptionManager = new InMemoryEventBusSubscriptionManager(e => e);
            var eventName = "TestCreatedIntegrationEvent";
            subscriptionManager.AddSubscription<TestCreatedIntegrationEvent, IIntegrationEventHandler<TestCreatedIntegrationEvent>>();

            // Act
            subscriptionManager.RemoveSubscription<TestCreatedIntegrationEvent, IIntegrationEventHandler<TestCreatedIntegrationEvent>>();

            // Assert
            Assert.False(subscriptionManager.HasSubscriptionsForEvent(eventName));
        }

        [Fact]
        public void GetHandlersForEvent_ShouldReturnHandlers()
        {
            // Arrange
            var subscriptionManager = new InMemoryEventBusSubscriptionManager(e => e);
            subscriptionManager.AddSubscription<TestCreatedIntegrationEvent, IIntegrationEventHandler<TestCreatedIntegrationEvent>>();

            // Act
            var handlers = subscriptionManager.GetHandlersForEvent<TestCreatedIntegrationEvent>();

            // Assert
            Assert.Single(handlers);
        }

        [Fact]
        public void HasSubscriptionsForEvent_WhenNoSubscriptions_ShouldReturnFalse()
        {
            // Arrange
            var subscriptionManager = new InMemoryEventBusSubscriptionManager(e => e);

            // Act & Assert
            Assert.False(subscriptionManager.HasSubscriptionsForEvent<TestCreatedIntegrationEvent>());
        }

        [Fact]
        public void Clear_ShouldRemoveAllHandlers()
        {
            // Arrange
            var subscriptionManager = new InMemoryEventBusSubscriptionManager(e => e);
            subscriptionManager.AddSubscription<TestCreatedIntegrationEvent, IIntegrationEventHandler<TestCreatedIntegrationEvent>>();

            // Act
            subscriptionManager.Clear();

            // Assert
            Assert.True(subscriptionManager.IsEmpty);
        }

        #endregion
    }
}