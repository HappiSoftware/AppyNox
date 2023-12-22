using AppyNox.Gateway.OcelotGateway.Middlewares;
using AppyNox.Services.Base.API.Middleware;
using NLog;
using NLog.Web;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

var builder = WebApplication.CreateBuilder(args);

#region [ SSL Configuration ]

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    string fileName = string.Empty;

    if (builder.Environment.IsDevelopment())
    {
        fileName = Directory.GetCurrentDirectory() + "/ssl/appynox.pfx";
    }
    else if (builder.Environment.IsStaging())
    {
        fileName = "../https/appynox.pfx";
    }
    else if (builder.Environment.IsProduction())
    {
        fileName = "/https/appynox.pfx";
    }

    Console.WriteLine($"SSL Certificate Path: {fileName}");

    // Check if the file exists and log the result
    if (File.Exists(fileName))
    {
        Console.WriteLine("SSL Certificate file found.");
        try
        {
            // Try to read the file (optional, as just checking existence might be sufficient)
            var fileContent = File.ReadAllText(fileName);
            Console.WriteLine("SSL Certificate file read successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading SSL Certificate file: {ex.Message}");
        }
    }
    else
    {
        Console.WriteLine("SSL Certificate file not found.");
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

#region [ Logger Setup ]

if (!builder.Environment.IsDevelopment())
{
    NLog.LogManager.Setup().LoadConfigurationFromFile($"Configurations/nlog.config");
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();
}

#endregion

builder.Services.AddOcelot(builder.Configuration).AddConsul();

var app = builder.Build();

app.UseMiddleware<CorrelationIdMiddleware>();

app.UseMiddleware<LoggingMiddleware>();

app.MapHealthChecks("/health");

await app.UseOcelot();

app.Run();