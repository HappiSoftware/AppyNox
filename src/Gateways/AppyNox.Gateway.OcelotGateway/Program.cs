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

Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger();

builder.Host.UseSerilog(Log.Logger);

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
        fileName = "/ssl/appynox.pfx";
    }

    if (!File.Exists(fileName))
    {
        throw new FileNotFoundException($"The SSL certificate file was not found at path: {fileName}");
    }

    serverOptions.ConfigureEndpointDefaults(listenOptions =>
    {
        listenOptions.UseHttps(fileName, "happi2024");
    });
});

#endregion

builder.Services.AddSingleton(typeof(INoxApiLogger<>), typeof(NoxApiLogger<>));

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

app.UseHealthChecks("/health");

await app.UseOcelot();

#endregion

await app.RunAsync();