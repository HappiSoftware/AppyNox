using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Base.Infrastructure.Helpers
{
    public static class DependencyInjectionHelper
    {
        #region [ Protected Methods ]

        public static void ApplyMigrations<TDbContext>(this IServiceProvider serviceProvider)
            where TDbContext : DbContext
        {
            using var scope = serviceProvider.CreateScope();
            var _db = scope.ServiceProvider.GetRequiredService<TDbContext>();

            if (_db.Database.GetPendingMigrations().Any())
            {
                _db.Database.Migrate();
            }
        }

        public static ILogger ConfigureLogging(IServiceCollection services, string name)
        {
            using ServiceProvider serviceProvider = services.BuildServiceProvider();
            ILoggerFactory? loggerFactory = serviceProvider.GetService<ILoggerFactory>()
                ?? throw new InvalidOperationException("LoggerFactory not available");
            return loggerFactory.CreateLogger(name);
        }

        #endregion
    }
}