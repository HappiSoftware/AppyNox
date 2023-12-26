using AppyNox.Gateway.OcelotGateway.Middlewares;
using AppyNox.Services.Base.API.Middleware;
using NLog;
using NLog.Web;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

var builder = WebApplication.CreateBuilder(args);

#region [ Logger Setup ]

NLog.LogManager.Setup().LoadConfigurationFromFile($"Configurations/nlog.config");
builder.Logging.ClearProviders();
builder.Host.UseNLog();

var logger = NLog.LogManager.GetCurrentClassLogger();

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
        logger.Info("SSL Certificate file found.");
    }
    else
    {
        logger.Info("SSL Certificate file not found.");
    }

    serverOptions.ConfigureEndpointDefaults(listenOptions =>
    {
        listenOptions.UseHttps(fileName ?? throw new InvalidOperationException("SSL certificate file path could not be determined."), "happi2023");
    });
});

#endregion

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile($"Configurations/ocelot.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddHealthChecks();

builder.Services.AddOcelot(builder.Configuration).AddConsul();

var app = builder.Build();

app.UseMiddleware<CorrelationIdMiddleware>();

app.UseMiddleware<LoggingMiddleware>();

app.MapHealthChecks("/health");

await app.UseOcelot();

app.Run();