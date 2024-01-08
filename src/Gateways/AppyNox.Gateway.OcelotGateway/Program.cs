using AppyNox.Gateway.OcelotGateway.Middlewares;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Infrastructure.Helpers;
using AppyNox.Services.Base.Infrastructure.Services.LoggerService;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

#region [ Configuration Service ]

await builder.AddConsulConfiguration("OcelotService");
await builder.AddConsulConfiguration("OcelotService", "ocelot");
var configuration = builder.Configuration;

#endregion

#region [ Logger Setup ]

builder.Host.UseSerilog((context, services, config) =>
    config.ReadFrom.Configuration(context.Configuration)
          .ReadFrom.Services(services)
);
builder.Services.AddSingleton<INoxApiLogger, NoxApiLogger>();

#region [ Logger for Before DI Initialization ]

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddSerilog();
});
var logger = loggerFactory.CreateLogger<INoxLogger>();
NoxLogger noxLogger = new(logger, "GatewayHost");

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

builder.Services.AddHealthChecks();

builder.Services.AddOcelot(builder.Configuration.GetSection("Gateway")).AddConsul();

var app = builder.Build();

#region [ Pipeline ]

app.UseMiddleware<LoggingMiddleware>();

app.UseMiddleware<OcelotCorrelationIdMiddleware>();

app.MapHealthChecks("/health");

await app.UseOcelot();

#endregion

app.Run();