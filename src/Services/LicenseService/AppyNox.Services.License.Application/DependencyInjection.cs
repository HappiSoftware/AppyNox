using AppyNox.Services.Base.Application.DtoUtilities;
using AppyNox.Services.Base.Application.Helpers;
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
        }

        #endregion
    }
}