using AppyNox.Services.Base.API.Constants;
using AppyNox.Services.Base.API.Extensions;
using AppyNox.Services.Base.API.Filters;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Infrastructure.Extensions;
using AppyNox.Services.Base.Infrastructure.Services.LoggerService;
using Asp.Versioning;
using Asp.Versioning.Conventions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Json;

namespace AppyNox.Services.Base.API;

public class ApiServiceOptions
{
#nullable disable
    [Required]
    public string SetupHostName { get; set; }
    public bool UseConsulKV { get; set; }
    public Action<IServiceCollection, INoxLogger, IConfiguration> ConfigureLayers { get; set; }
    public IEnumerable<string> Versions { get; set; } = [NoxVersions.v1_0];
    public bool UseDynamicRequestBodyOperationFilter { get; set; } = true;

#nullable enable
}

public static class ApiServiceBuilder
{
    public async static Task<WebApplicationBuilder> AddApiServices(this WebApplicationBuilder builder,
        Action<ApiServiceOptions> configureOptions)
    {
        ApiServiceOptions options = new();
        configureOptions(options);
        string serviceName = builder.Configuration["Consul:ServiceName"]
            ?? throw new InvalidOperationException("Consul:ServiceName is not defined!");

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(options);
        if (!Validator.TryValidateObject(options, validationContext, validationResults, true))
        {
            var errors = string.Join(", ", validationResults.Select(vr => vr.ErrorMessage));
            throw new InvalidOperationException($"Invalid options: {errors}");
        }

        var configuration = builder.Configuration;
        NoxLogger<WebApplicationBuilder> logger = builder.SetUpLogger();

        if (options.UseConsulKV)
        {
            await builder.AddConsulConfiguration(configuration["Consul:ServiceName"] 
                ?? throw new InvalidOperationException("Consul:ServiceName was empty!"));

            logger.LogInformation($"-{serviceName}- Consul KV enabled...", false);
        }

        builder.ConfigureServices();
        logger.LogInformation($"-{serviceName}- Services configured...", false);

        options.ConfigureLayers?.Invoke(builder.Services, logger, configuration);
        logger.LogInformation($"-{serviceName}- Layers configured...", false);

        builder.ConfigureSwagger(options);
        logger.LogInformation($"-{serviceName}- Swagger configured...", false);

        return builder;
    }

    private static NoxLogger<WebApplicationBuilder> SetUpLogger(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger();

        builder.Host.UseSerilog(Log.Logger);

        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddSerilog(Log.Logger);
        });
        var logger = loggerFactory.CreateLogger<WebApplicationBuilder>();
        return new(logger, $"{builder.Configuration["Consul:ServiceName"]}Host");
    }

    private static void ConfigureServices(this WebApplicationBuilder builder)
    {
        // Add services to the container.
        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddHealthChecks();

        builder.Services.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        }).AddMvc(options =>
        {
            options.Conventions.Add(new VersionByNamespaceConvention());
        });

        builder.ConfigureLocalization();
    }

    private static void ConfigureSwagger(this WebApplicationBuilder builder, ApiServiceOptions options)
    {
        var serviceName = builder.Configuration["Consul:ServiceName"];
        builder.Services.AddSwaggerGen(opt =>
        {
            if(options.UseDynamicRequestBodyOperationFilter)
            {
                opt.OperationFilter<DynamicRequestBodyOperationFilter>();
            }

            foreach(var version in options.Versions)
            {
                opt.SwaggerDoc($"v{version}", new OpenApiInfo { Title = serviceName, Version = version });
            }

            opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });

            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });

            opt.DocInclusionPredicate((version, apiDesc) =>
            {
                if (!apiDesc.TryGetMethodInfo(out MethodInfo methodInfo)) return false;

                var versions = methodInfo.DeclaringType?
                    .GetCustomAttributes(true)
                    .OfType<ApiVersionAttribute>()
                    .SelectMany(attr => attr.Versions);

                return versions?.Any(v => $"v{v}" == version) ?? false;
            });
        });
    }
}