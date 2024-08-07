using AppyNox.Gateway.OcelotGateway.Middlewares;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Infrastructure.Extensions;
using AppyNox.Services.Base.Infrastructure.Services.LoggerService;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

#region [ Configuration Service ]

var configuration = builder.Configuration;
await builder.AddConsulConfiguration("OcelotService");
await builder.AddConsulConfiguration("OcelotService", "ocelot");

#endregion

#region [ Logger Setup ]

builder.Host.UseSerilog((context, services, config) =>
    config.ReadFrom.Configuration(context.Configuration)
          .ReadFrom.Services(services)
);
builder.Services.AddSingleton(typeof(INoxApiLogger<>), typeof(NoxApiLogger<>));

#region [ Logger for Before DI Initialization ]

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddSerilog();
});
var logger = loggerFactory.CreateLogger<WebApplicationBuilder>();
NoxLogger<WebApplicationBuilder> noxLogger = new(logger, "GatewayHost");

#endregion

#endregion

#region [ SSL Configuration ]

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    string fileName = string.Empty;

    if (builder.Environment.IsDevelopment())
    {
        fileName = Directory.GetCurrentDirectory() + "/ssl/appynox.pfx";
    }
    else if (builder.Environment.IsStaging() || builder.Environment.IsProduction())
    {
        fileName = "/https2/appynox.pfx";
    }

    // Check if the file exists and log the result
    if (File.Exists(fileName))
    {
        noxLogger.LogInformation($"SSL Certificate file found at {fileName}.");
    }
    else
    {
        noxLogger.LogWarning($"SSL Certificate file not found at {fileName}.");
    }

    serverOptions.ConfigureEndpointDefaults(listenOptions =>
    {
        listenOptions.UseHttps(fileName ?? throw new InvalidOperationException("SSL certificate file path could not be determined."), "happi2023");
    });
});

#endregion

#region [ CORS ]

string[]? allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();

if (allowedOrigins != null && allowedOrigins.Length > 0)
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowSpecificOrigin",
            builder => builder.WithOrigins(allowedOrigins)
                              .AllowAnyHeader()
                              .AllowAnyMethod());
    });
}

#endregion

builder.Services.AddHealthChecks();

builder.Services.AddOcelot(builder.Configuration.GetSection("Gateway")).AddConsul();

var app = builder.Build();

#region [ Pipeline ]

app.UseHsts();

if (allowedOrigins != null && allowedOrigins.Length > 0)
{
    app.UseCors("AllowSpecificOrigin");
}

app.UseMiddleware<LoggingMiddleware>();

app.UseMiddleware<OcelotCorrelationIdMiddleware>();

app.MapHealthChecks("/health");

await app.UseOcelot();

#endregion

app.Run();