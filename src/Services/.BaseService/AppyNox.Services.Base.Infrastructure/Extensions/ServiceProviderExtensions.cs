using AppyNox.Services.Base.Infrastructure.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace AppyNox.Services.Base.Infrastructure.Extensions
{
    /// <summary>
    /// Provides helper methods for dependency injection and service configuration.
    /// </summary>
    public static class DependencyInjectionHelper
    {
        #region [ Public Methods ]

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

        public static void InitializeNoxInfrastructureLocalizationService(this IStringLocalizerFactory localizerFactory)
        {
            NoxInfrastructureResourceService.Initialize(localizerFactory);
        }

        #endregion
    }
}