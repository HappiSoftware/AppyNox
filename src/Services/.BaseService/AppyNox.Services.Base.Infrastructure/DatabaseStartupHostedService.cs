using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Base.Infrastructure
{
    [Obsolete("DatabaseStartupHostedService is deprecated for now. Database wait ops for apis moved to docker-compose-wait. Might need to consider usage later in future.")]
    public class DatabaseStartupHostedService<TContext>(IServiceProvider serviceProvider, ILogger<DatabaseStartupHostedService<TContext>> logger) : IHostedService where TContext : DbContext
    {
        #region [ Fields ]

        private readonly IServiceProvider _serviceProvider = serviceProvider;

        private readonly ILogger<DatabaseStartupHostedService<TContext>> _logger = logger;

        #endregion

        #region [ Events ]

        public event Func<Task>? OnDatabaseConnected;

        public event Func<Task>? OnDatabaseConnectionFailed;

        #endregion

        #region [ Public Methods ]

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                await WaitForDatabaseAsync(_serviceProvider, _logger, cancellationToken);
                OnDatabaseConnected?.Invoke();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "{Message}", $"Unexpected error thrown");
                OnDatabaseConnectionFailed?.Invoke();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        #endregion

        #region [ Private Methods ]

        private static async Task WaitForDatabaseAsync(IServiceProvider serviceProvider, ILogger logger, CancellationToken cancellationToken, int maxAttempts = 10)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();

            logger.LogInformation("{Message}", $"Attempting to connect to the database {dbContext.Database.GetDbConnection().ConnectionString}");

            int attempts = 0;

            while (attempts < maxAttempts)
            {
                try
                {
                    if (await dbContext.Database.CanConnectAsync(cancellationToken))
                    {
                        logger.LogInformation("{Message}", $"Successfully connected to the database {dbContext.Database.GetDbConnection().ConnectionString}");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "{Message}", $"Failed to connect to the database {dbContext.Database.GetDbConnection().ConnectionString}");
                }

                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
                attempts++;
            }

            logger.LogError("{Message}", $"Failed to connect to the database after {attempts} attempts. {dbContext.Database.GetDbConnection().ConnectionString}");
            throw new InvalidOperationException("Unable to connect to the database.");
        }

        #endregion
    }
}