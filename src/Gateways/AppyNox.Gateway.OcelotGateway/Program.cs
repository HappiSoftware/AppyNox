using AppyNox.Gateway.OcelotGateway.Middlewares;
using NLog;
using NLog.Web;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

var logger = NLog.LogManager.Setup().LoadConfigurationFromFile("Configurations/nlog.config").GetCurrentClassLogger();
logger.Info("--- init main ---");

try
{
    var builder = WebApplication.CreateBuilder(args);

    #region [ SSL Configuration ]

    builder.WebHost.ConfigureKestrel(serverOptions =>
    {
        string fileName = string.Empty;

        if (builder.Environment.IsDevelopment())
        {
            fileName = Directory.GetCurrentDirectory() + "/ssl/gateway.pfx";
        }
        else if (builder.Environment.IsProduction())
        {
            fileName = "/https/gateway.pfx";
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

    #region [ Logger Setup ]

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    #endregion

    builder.Services.AddOcelot(builder.Configuration).AddConsul();

    var app = builder.Build();

    app.UseMiddleware<LoggingMiddleware>();
    
    await app.UseOcelot();

    app.Run();
}
catch (Exception e)
{
    logger.Error(e, "Stopped program because of exception");
    throw;
}
finally
{
    NLog.LogManager.Shutdown();
}