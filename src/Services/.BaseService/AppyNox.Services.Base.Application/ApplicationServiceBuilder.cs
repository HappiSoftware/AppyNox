using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Application.MediatR.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace AppyNox.Services.Base.Application;


public class ApplicationSetupOptions
{
#nullable disable
    [Required]
    public string Assembly { get; set; }
    [Required]
    public IConfiguration Configuration { get; set; }
    public bool UseAutoMapper { get; set; } = true;
    public bool UseFluentValidation { get; set; } = true;
    public bool UseDtoMappingRegistry { get; set; } = true;
    public bool UseMediatR { get; set; } = true;

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
        string serviceName = options.Configuration["Consul:ServiceName"]
            ?? throw new InvalidOperationException("Consul:ServiceName is not defined!");

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
            logger.LogInformation($"-{serviceName}- AutoMapper enabled...", false);
        }

        if (options.UseFluentValidation)
        {
            services.AddValidatorsFromAssembly(applicationAssembly);
            logger.LogInformation($"-{serviceName}- FluentValidation enabled...", false);
        }

        if(options.UseMediatR)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(applicationAssembly);
            });
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(NoxCommandActionBehavior<,>));
            logger.LogInformation($"-{serviceName}- MediatR enabled...", false);
        }

        return services;
    }
}