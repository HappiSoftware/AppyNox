using AppyNox.Services.Base.Application.DtoUtilities;
using AppyNox.Services.Base.Application.Logger;
using AppyNox.Services.License.Application.Dtos.DtoUtilities;
using AppyNox.Services.License.Application.Services.Implementations;
using AppyNox.Services.License.Application.Services.Interfaces;
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
            services.AddAutoMapper(Assembly.Load("AppyNox.Services.License.Application"));
            services.AddValidatorsFromAssembly(Assembly.Load("AppyNox.Services.License.Application"));

            services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
            services.AddSingleton(typeof(IDtoMappingRegistryBase), typeof(DtoMappingRegistry));
            services.AddSingleton<INoxApplicationLogger, NoxApplicationLogger>();
        }

        #endregion
    }
}