using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AppyNox.Services.Authentication.Application
{
    /// <summary>
    /// Provides extension methods for IServiceCollection to register authentication services and configurations.
    /// </summary>
    public static class DependencyInjection
    {
        #region [ Public Methods ]

        /// <summary>
        /// Adds authentication services and AutoMapper configurations to the specified IServiceCollection.
        /// </summary>
        /// <param name="services">The IServiceCollection to add services to.</param>
        /// <param name="configuration">The IConfiguration instance to access application settings.</param>
        public static void AddAuthenticationApplication(this IServiceCollection services, IConfiguration configuration)
        {
        }

        #endregion
    }
}