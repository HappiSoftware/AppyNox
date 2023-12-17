﻿using AppyNox.Services.Authentication.Application.Dtos.DtoUtilities;
using AppyNox.Services.Authentication.Application.Services.Implementations;
using AppyNox.Services.Authentication.Application.Services.Interfaces;
using AppyNox.Services.Base.Application.DtoUtilities;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AppyNox.Services.Authentication.Application
{
    public static class DependencyInjection
    {
        #region [ Public Methods ]

        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.Load("AppyNox.Services.Authentication.Application"));
            services.AddValidatorsFromAssembly(Assembly.Load("AppyNox.Services.Authentication.Application"));

            services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
            services.AddSingleton(typeof(IDtoMappingRegistryBase), typeof(DtoMappingRegistry));
        }

        #endregion
    }
}