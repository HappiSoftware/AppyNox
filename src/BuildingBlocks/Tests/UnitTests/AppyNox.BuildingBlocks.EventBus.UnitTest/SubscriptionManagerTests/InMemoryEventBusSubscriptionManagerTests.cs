using AppyNox.BuildingBlocks.EventBus.UnitTest.Events;
using AppyNox.EventBus.Base.Abstraction;
using AppyNox.EventBus.Base.SubscriptionManagers;

namespace AppyNox.BuildingBlocks.EventBus.UnitTest.SubscriptionManagerTests
{
    public class InMemoryEventBusSubscriptionManagerTests
    {
        #region Public Methods

        [Fact]
        public void GetEventKeyShouldReturnFormatted()
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
        public void AddSubscriptionShouldAddHandler()
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
        public void RemoveSubscriptionShouldRemoveHandler()
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
        public void GetHandlersForEventShouldReturnHandlers()
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
        public void HasSubscriptionsForEventWhenNoSubscriptionsShouldReturnFalse()
        {
            // Arrange
            var subscriptionManager = new InMemoryEventBusSubscriptionManager(e => e);

            // Act & Assert
            Assert.False(subscriptionManager.HasSubscriptionsForEvent<TestCreatedIntegrationEvent>());
        }

        [Fact]
        public void ClearShouldRemoveAllHandlers()
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