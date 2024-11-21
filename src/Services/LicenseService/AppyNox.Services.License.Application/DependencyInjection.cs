using AppyNox.Services.Base.Application;
using AppyNox.Services.Base.Application.Extensions;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.License.Application.Dtos.DtoUtilities;
using AppyNox.Services.License.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace AppyNox.Services.License.Application
{
    public static class DependencyInjection
    {
        #region [ Public Methods ]

        public static IHostApplicationBuilder AddLicenseApplication(this IHostApplicationBuilder builder, IConfiguration configuration, INoxLogger logger)
        {
            IServiceCollection services = builder.Services;
            services.AddApplicationServices(logger, options =>
            {
                options.Assembly = Assembly.GetExecutingAssembly().GetName().Name;
                options.Configuration = configuration;
                options.UseAutoMapper = true;
                options.UseFluentValidation = true;
                options.UseDtoMappingRegistry = true;
                options.DtoMappingRegistryFactory = provider => new DtoMappingRegistry();
                options.UseMediatR = true;
            });

            services.AddNoxEntityCommands<LicenseEntity, LicenseId>();
            services.AddNoxEntityCommands<ProductEntity, ProductId>();

            return builder;
        }

        #endregion
    }
}