using AppyNox.Services.Base.Application.DtoUtilities;
using AppyNox.Services.Base.Application.Exceptions.Base;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Application.MediatR.Behaviors;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace AppyNox.Services.Base.Application;


public class ApplicationSetupOptions
{
#nullable disable
    [Required]
    public string Assembly { get; set; }
    public bool UseAutoMapper { get; set; } = true;
    public bool UseFluentValidation { get; set; } = true;
    public bool UseDtoMappingRegistry { get; set; } = true;
    public bool UseMediatR { get; set; } = true;
    public Func<IServiceProvider, IDtoMappingRegistryBase> DtoMappingRegistryFactory { get; set; }


#nullable enable
}

public static class ApplicationServiceBuilder
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        INoxLogger logger,
        Action<ApplicationSetupOptions> configureOptions)
    {
        var options = new ApplicationSetupOptions();
        configureOptions(options);

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(options);
        if (!Validator.TryValidateObject(options, validationContext, validationResults, true))
        {
            var errors = string.Join(", ", validationResults.Select(vr => vr.ErrorMessage));
            throw new InvalidOperationException($"Invalid options: {errors}");
        }

        Assembly applicationAssembly = Assembly.Load(options.Assembly);

        if (options.UseAutoMapper)
        {
            services.AddAutoMapper(applicationAssembly);
            logger.LogInformation("AutoMapper enabled...");
        }

        if (options.UseFluentValidation)
        {
            services.AddValidatorsFromAssembly(applicationAssembly);
            logger.LogInformation("FluentValidation enabled...");
        }

        if (options.UseDtoMappingRegistry)
        {
            if (options.DtoMappingRegistryFactory is null)
            {
                throw new NoxApplicationException("DtoMappingRegistryFactory was null!");
            }
            services.AddSingleton(typeof(IDtoMappingRegistryBase), options.DtoMappingRegistryFactory);
            logger.LogInformation("DtoMappingRegistry enabled...");
        }

        if(options.UseMediatR)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(applicationAssembly);
            });
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(NoxCommandActionBehavior<,>));
            logger.LogInformation("MediatR enabled...");
        }

        return services;
    }
}