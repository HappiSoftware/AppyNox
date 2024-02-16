using AppyNox.Services.Base.Application.DtoUtilities;
using AppyNox.Services.Base.Application.Extensions;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.License.Application.Dtos.DtoUtilities;
using AppyNox.Services.License.Domain.Entities;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AppyNox.Services.License.Application
{
    public static class DependencyInjection
    {
        #region [ Public Methods ]

        public static void AddLicenseApplication(this IServiceCollection services)
        {
            Assembly applicationAssembly = Assembly.Load("AppyNox.Services.License.Application");
            services.AddAutoMapper(applicationAssembly);
            services.AddValidatorsFromAssembly(applicationAssembly);

            #region [ CQRS ]

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(applicationAssembly);
            });
            services.AddGenericEntityCommandHandlers(
                (typeof(LicenseEntity), typeof(LicenseId)),
                (typeof(ProductEntity), typeof(ProductId))
                );

            #endregion

            services.AddSingleton(typeof(IDtoMappingRegistryBase), typeof(DtoMappingRegistry));
        }

        #endregion
    }
}