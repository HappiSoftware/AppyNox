﻿using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.EventBus.RabbitMQ
{
    public class RabbitMQPersistentConnection : IDisposable
    {
        #region [ Fields ]

        private readonly IConnectionFactory _connectionFactory;

        private readonly int _retryCount;

        private IConnection connection;

        private object lock_object = new object();

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
            return connection.CreateModel();
        }

        public void Dispose()
        {
            _disposed = true;
            connection.Dispose();
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

                if (IsConnected)
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