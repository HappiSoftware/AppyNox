using AppyNox.Services.Base.Application;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
        /// <param name="builder">The IServiceCollection to add services to.</param>
        /// <param name="configuration">The IConfiguration instance to access application settings.</param>
        public static IHostApplicationBuilder AddSsoApplication(
            this IHostApplicationBuilder builder,
            IConfiguration configuration,
            INoxLogger logger
        )
        {
            IServiceCollection services = builder.Services;
            services.AddApplicationServices(logger, options =>
            {
                options.Assembly = Assembly.GetExecutingAssembly().GetName().Name;
                options.Configuration = configuration;
                options.UseAutoMapper = true;
                options.UseFluentValidation = true;
                options.UseDtoMappingRegistry = false;
                options.UseMediatR = true;
            });

            return builder;
        }

        #endregion
    }
}
