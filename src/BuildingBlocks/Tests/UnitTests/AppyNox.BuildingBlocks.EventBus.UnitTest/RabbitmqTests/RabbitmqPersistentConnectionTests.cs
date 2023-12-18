using AppyNox.EventBus.Base.ExceptionExtensions.Base;
using AppyNox.EventBus.RabbitMQ;
using AppyNox.Services.Base.Domain.Common.HttpStatusCodes;
using Moq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AppyNox.BuildingBlocks.EventBus.UnitTest.RabbitmqTests
{
    public class RabbitmqPersistentConnectionTests
    {
        #region Public Methods

        [Fact]
        public void CreateModel_ShouldReturnModel()
        {
            var connectionFactoryMock = new Mock<IConnectionFactory>();
            var connectionMock = new Mock<IConnection>();
            var modelMock = new Mock<IModel>();

            connectionMock.Setup(conn => conn.CreateModel()).Returns(modelMock.Object);
            connectionFactoryMock.Setup(cf => cf.CreateConnection()).Returns(connectionMock.Object);

            var persistentConnection = new RabbitMQPersistentConnection(connectionFactoryMock.Object, 1);
            persistentConnection.TryConnect();

            var model = persistentConnection.CreateModel();

            Assert.NotNull(model);
            Assert.IsAssignableFrom<IModel>(model);

            connectionMock.Verify(conn => conn.CreateModel(), Times.Once);
        }

        [Fact]
        public void CreateModel_WithoutConnection_ShouldThrowEventBusBaseException()
        {
            var connectionFactoryMock = new Mock<IConnectionFactory>();

            // RabbitMQPersistentConnection without IConnection setup
            var persistentConnection = new RabbitMQPersistentConnection(connectionFactoryMock.Object, 1);

            var exception = Assert.Throws<EventBusBaseException>(() => persistentConnection.CreateModel());

            Assert.Equal((int)NoxServerErrorResponseCodes.InternalServerError, exception.StatusCode);
        }

        [Fact]
        public void TryConnect_WithSuccessfulConnection_ShouldReturnTrue()
        {
            var connectionFactoryMock = new Mock<IConnectionFactory>();
            var connectionMock = new Mock<IConnection>();

            connectionMock.SetupGet(c => c.IsOpen).Returns(true);

            connectionFactoryMock.Setup(cf => cf.CreateConnection()).Returns(connectionMock.Object);

            var retryCount = 1;
            var persistentConnection = new RabbitMQPersistentConnection(connectionFactoryMock.Object, retryCount);

            var result = persistentConnection.TryConnect();

            Assert.True(result);
            connectionMock.Verify(conn => conn.CreateModel(), Times.Never); // CreateModel should not be called
            connectionFactoryMock.Verify(cf => cf.CreateConnection(), Times.Once); // CreateConnection should be called once

            connectionMock.VerifyAdd(conn => conn.ConnectionShutdown += It.IsAny<EventHandler<ShutdownEventArgs>>(), Times.Once);
            connectionMock.VerifyAdd(conn => conn.CallbackException += It.IsAny<EventHandler<CallbackExceptionEventArgs>>(), Times.Once);
            connectionMock.VerifyAdd(conn => conn.ConnectionBlocked += It.IsAny<EventHandler<ConnectionBlockedEventArgs>>(), Times.Once);
        }

        [Fact]
        public void TryConnect_WithoutConnection_ShouldFail()
        {
            var connectionFactoryMock = new Mock<IConnectionFactory>();
            var connectionMock = new Mock<IConnection>();

            connectionMock.SetupGet(c => c.IsOpen).Returns(false);

            connectionFactoryMock.Setup(cf => cf.CreateConnection()).Returns(connectionMock.Object);

            var retryCount = 1;
            var persistentConnection = new RabbitMQPersistentConnection(connectionFactoryMock.Object, retryCount);

            var result = persistentConnection.TryConnect();

            Assert.False(result);
        }

        [Fact]
        public void Dispose_WhenConnectionIsNull_ShouldNotThrow()
        {
            var connectionFactoryMock = new Mock<IConnectionFactory>();

            // RabbitMQPersistentConnection without IConnection setup
            var persistentConnection = new RabbitMQPersistentConnection(connectionFactoryMock.Object, 1);

            var exceptionRecord = Record.Exception(() => persistentConnection.Dispose());

            Assert.Null(exceptionRecord); // Dispose sırasında hata fırlatılmamalı
        }

        #endregion
    }
}