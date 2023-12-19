using AppyNox.Services.Base.Domain.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace AppyNox.Services.Base.API.Helpers
{
    public static class EnvironmentHelpers
    {
        #region [ Public Methods ]

        /// <summary>
        /// Returns the current environment to ApplicationEnvironment enum type. Makes it possible to use the enum in class projects.
        /// </summary>
        /// <param name="environment"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
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