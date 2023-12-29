using AppyNox.Services.Base.Domain.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace AppyNox.Services.Base.API.Helpers
{
    /// <summary>
    /// Provides helper methods for working with application environments.
    /// </summary>
    public static class EnvironmentHelpers
    {
        #region [ Public Methods ]

        /// <summary>
        /// Determines the application environment based on the web host environment settings.
        /// </summary>
        /// <param name="environment">The web host environment.</param>
        /// <returns>The determined application environment.</returns>
        public static ApplicationEnvironment GetEnvironment(this IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                return ApplicationEnvironment.Development;
            }
            else if (environment.IsProduction())
            {
                return ApplicationEnvironment.Production;
            }
            else if (environment.IsStaging())
            {
                return ApplicationEnvironment.Staging;
            }
            else
            {
                throw new InvalidOperationException("Environment could not be determined.");
            }
        }

        #endregion
    }
}