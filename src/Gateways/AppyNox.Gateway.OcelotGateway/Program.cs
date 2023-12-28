using AppyNox.Gateway.OcelotGateway.Middlewares;
using AppyNox.Services.Base.API.Middleware;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

#region [ Logger Setup ]

builder.Host.UseSerilog((context, services, config) =>
    config.ReadFrom.Configuration(context.Configuration)
          .ReadFrom.Services(services)
          .Enrich.FromLogContext()
          .WriteTo.Console()
          .WriteTo.File("logs/appynox.txt", rollingInterval: RollingInterval.Day)
);

var loggerFactory = LoggerFactory.Create(loggingBuilder =>
{
    loggingBuilder.AddSerilog();
});
var logger = loggerFactory.CreateLogger<Program>();

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
        logger.LogInformation("SSL Certificate file found at {FilePath}.", fileName);
    }
    else
    {
        logger.LogWarning("SSL Certificate file not found at {FilePath}.", fileName);
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