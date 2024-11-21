using AppyNox.Services.Base.API.Constants;
using AppyNox.Services.Base.API.Extensions;
using AppyNox.Services.Base.API.Filters;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Infrastructure.Extensions;
using AppyNox.Services.Base.Infrastructure.Services.LoggerService;
using Asp.Versioning;
using Asp.Versioning.Conventions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
    public Action<INoxLogger, IConfiguration> ConfigureLayers { get; set; }
    public IEnumerable<string> Versions { get; set; } = [NoxVersions.v1_0];
    public bool UseDynamicRequestBodyOperationFilter { get; set; } = true;

#nullable enable
}

public static class ApiServiceBuilder
{
    public async static Task<IHostApplicationBuilder> AddApiServices(this IHostApplicationBuilder builder,
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
        NoxLogger<IHostApplicationBuilder> logger = builder.SetUpLogger();

        if (options.UseConsulKV)
        {
            await builder.AddConsulConfiguration(configuration["Consul:ServiceName"] 
                ?? throw new InvalidOperationException("Consul:ServiceName was empty!"));

            logger.LogInformation($"-{serviceName}- Consul KV enabled...", false);
        }

        builder.ConfigureServices();
        logger.LogInformation($"-{serviceName}- Services configured...", false);

        options.ConfigureLayers?.Invoke(logger, configuration);
        logger.LogInformation($"-{serviceName}- Layers configured...", false);

        builder.ConfigureSwagger(options);
        logger.LogInformation($"-{serviceName}- Swagger configured...", false);

        return builder;
    }

    private static NoxLogger<IHostApplicationBuilder> SetUpLogger(this IHostApplicationBuilder builder)
    {
        string seqServerUrl = builder.Configuration.GetConnectionString("appynox-seq") ?? 
            throw new Exception("Seq connection string could not found.");

        var logger = Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .WriteTo.Seq(seqServerUrl)
            // TODO below comes from this thread: https://stackoverflow.com/questions/78369387/how-to-wire-up-serilog-to-net-aspire
            .WriteTo.OpenTelemetry(options =>
            {
                options.Endpoint = builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"];
                var headers = builder.Configuration["OTEL_EXPORTER_OTLP_HEADERS"]?.Split(',') ?? [];
                foreach (var header in headers)
                {
                    var (key, value) = header.Split('=') switch
                    {
                    [string k, string v] => (k, v),
                        var v => throw new Exception($"Invalid header format {v}")
                    };

                    options.Headers.Add(key, value);
                }
                options.ResourceAttributes.Add("service.name", "apiservice");

                //To remove the duplicate issue, we can use the below code to get the key and value from the configuration
                var (otelResourceAttribute, otelResourceAttributeValue) = builder.Configuration["OTEL_RESOURCE_ATTRIBUTES"]?.Split('=') switch
                {
                [string k, string v] => (k, v),
                    _ => throw new Exception($"Invalid header format {builder.Configuration["OTEL_RESOURCE_ATTRIBUTES"]}")
                };

                options.ResourceAttributes.Add(otelResourceAttribute, otelResourceAttributeValue);

            })
            .CreateLogger();
        builder.AddSeqEndpoint("seq");

        builder.Logging.AddSerilog(logger);

        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddSerilog(Log.Logger);
        });

        var ilogger = loggerFactory.CreateLogger<IHostApplicationBuilder>();
        return new(ilogger, $"{builder.Configuration["Consul:ServiceName"]}Host");
    }

    private static void ConfigureServices(this IHostApplicationBuilder builder)
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

    private static void ConfigureSwagger(this IHostApplicationBuilder builder, ApiServiceOptions options)
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