using AppyNox.Services.Base.Application.DtoUtilities;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AppyNox.Services.Sso.Application
{
    /// <summary>
    /// Provides extension methods for IServiceCollection to register sso services and configurations.
    /// </summary>
    public static class DependencyInjection
    {
        #region [ Public Methods ]

        /// <summary>
        /// Adds Sso services and AutoMapper configurations to the specified IServiceCollection.
        /// </summary>
        /// <param name="services">The IServiceCollection to add services to.</param>
        /// <param name="configuration">The IConfiguration instance to access application settings.</param>
        public static void AddSsoApplication(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            Assembly applicationAssembly = Assembly.Load("AppyNox.Services.Sso.Application");
            services.AddAutoMapper(applicationAssembly);
            services.AddValidatorsFromAssembly(applicationAssembly);

            #region [ CQRS ]

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(applicationAssembly);
            });

            #endregion
        }

        #endregion
    }
}
