using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Base.Infrastructure
{
    public class DatabaseStartupHostedService<TContext>(IServiceProvider serviceProvider) : IHostedService where TContext : DbContext
    {
        #region [ Fields ]

        private readonly IServiceProvider _serviceProvider = serviceProvider;

        #endregion

        #region Events

        public event Func<Task>? OnDatabaseConnected;

        #endregion

        #region [ Public Methods ]

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                await WaitForDatabaseAsync(_serviceProvider, cancellationToken);
                OnDatabaseConnected?.Invoke();
            }
            catch (Exception)
            {
                // Log or handle the exception as necessary
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        #endregion

        #region [ Private Methods ]

        private static async Task WaitForDatabaseAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken, int maxAttempts = 10)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();
            int attempts = 0;

            while (attempts < maxAttempts)
            {
                try
                {
                    if (await dbContext.Database.CanConnectAsync(cancellationToken))
                    {
                        return;
                    }
                }
                catch
                {
                    // Log or handle the exception as necessary
                }

                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
                attempts++;
            }

            throw new InvalidOperationException("Unable to connect to the database.");
        }

        #endregion
    }
}