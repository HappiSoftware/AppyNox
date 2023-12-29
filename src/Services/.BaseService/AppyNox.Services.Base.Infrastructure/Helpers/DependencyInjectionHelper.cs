using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AppyNox.Services.Base.Infrastructure.Helpers
{
    /// <summary>
    /// Provides helper methods for dependency injection and service configuration.
    /// </summary>
    public static class DependencyInjectionHelper
    {
        #region [ Protected Methods ]

        /// <summary>
        /// Applies any pending database migrations for a given DbContext.
        /// </summary>
        /// <typeparam name="TDbContext">The type of the DbContext.</typeparam>
        /// <param name="serviceProvider">The service provider instance.</param>
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

        #endregion
    }
}