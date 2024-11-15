using AppyNox.Services.Base.Application;
using AppyNox.Services.Base.Application.Extensions;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.License.Application.Dtos.LicenseDtos.Models;
using AppyNox.Services.License.Application.Dtos.ProductDtos.Models;
using AppyNox.Services.License.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AppyNox.Services.License.Application
{
    public static class DependencyInjection
    {
        #region [ Public Methods ]

        public static IServiceCollection AddLicenseApplication(this IServiceCollection services, IConfiguration configuration, INoxLogger logger)
        {
            services.AddApplicationServices(logger, options =>
            {
                options.Assembly = Assembly.GetExecutingAssembly().GetName().Name;
                options.Configuration = configuration;
                options.UseAutoMapper = true;
                options.UseFluentValidation = true;
                options.UseDtoMappingRegistry = true;
                options.UseMediatR = true;
            });

            services.AddNoxEntityCommands<LicenseEntity, LicenseId, LicenseCreateDto, LicenseDto>();
            services.AddNoxEntityCommands<ProductEntity, ProductId, ProductCreateDto, ProductDto>();

            return services;
        }

        #endregion
    }
}