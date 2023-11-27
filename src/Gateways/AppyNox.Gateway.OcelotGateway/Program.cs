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

    builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
        .AddJsonFile($"Configurations/ocelot.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();

    #region [ Logger Setup ]

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    #endregion

    builder.Services.AddOcelot(builder.Configuration).AddConsul();

    var app = builder.Build();

    await app.UseOcelot();

    app.UseMiddleware<LoggingMiddleware>();

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