using AppyNox.EventBus.Base.ExceptionExtensions.Base;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Net;
using System.Net.Sockets;

namespace AppyNox.EventBus.RabbitMQ
{
    public sealed class RabbitMQPersistentConnection : IDisposable
    {
        #region [ Fields ]

        private readonly IConnectionFactory _connectionFactory;

        private readonly int _retryCount;

        private readonly object lock_object = new();

        private IConnection? connection;

        private bool _disposed;

        #endregion

        #region [ Public Constructors ]

        public RabbitMQPersistentConnection(IConnectionFactory connectionFactory, int retryCount)
        {
            _connectionFactory = connectionFactory;
            _retryCount = retryCount;
        }

        #endregion

        #region [ Properties ]

        public bool IsConnected => connection != null && connection.IsOpen;

        #endregion

        #region [ Public Methods ]

        public IModel CreateModel()
        {
            if (connection == null)
            {
                throw new EventBusBaseException("RabbitMQ connection is null while trying CreateModel", (int)HttpStatusCode.InternalServerError);
            }
            return connection.CreateModel();
        }

        public void Dispose()
        {
            _disposed = true;
            if (connection != null)
            {
                connection.Dispose();
            }
            GC.SuppressFinalize(this);
        }

        public bool TryConnect()
        {
            lock (lock_object)
            {
                var policy = Policy.Handle<SocketException>()
                    .Or<BrokerUnreachableException>()
                    .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                    {
                    });

                policy.Execute(() =>
                {
                    connection = _connectionFactory.CreateConnection();
                });

                if (IsConnected && connection != null)
                {
                    connection.ConnectionShutdown += Connection_ConnectionShutdown;
                    connection.CallbackException += Connection_CallbackException;
                    connection.ConnectionBlocked += Connection_ConnectionBlocked;

                    // log
                    return true;
                }

                return false;
            }
        }

        private void Connection_ConnectionBlocked(object? sender, ConnectionBlockedEventArgs e)
        {
            if (_disposed) { return; }

            TryConnect();
        }

        private void Connection_CallbackException(object? sender, CallbackExceptionEventArgs e)
        {
            if (_disposed) { return; }

            TryConnect();
        }

        private void Connection_ConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            if (_disposed) { return; }

            TryConnect();
        }

        #endregion
    }
}