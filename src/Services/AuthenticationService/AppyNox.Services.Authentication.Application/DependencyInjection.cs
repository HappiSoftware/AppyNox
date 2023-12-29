using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AppyNox.Services.Authentication.Application
{
    public static class DependencyInjection
    {
        #region [ Public Methods ]

        public static void AddAuthenticationApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.Load("AppyNox.Services.Authentication.Application"));
            services.AddValidatorsFromAssembly(Assembly.Load("AppyNox.Services.Authentication.Application"));
        }

        #endregion
    }
}