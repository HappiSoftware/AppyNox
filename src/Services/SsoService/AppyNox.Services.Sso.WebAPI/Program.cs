using AppyNox.Services.Base.API.Constants;
using AppyNox.Services.Base.API.Extensions;
using AppyNox.Services.Base.API.Middleware.Options;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Infrastructure.Extensions;
using AppyNox.Services.Base.Infrastructure.HostedServices;
using AppyNox.Services.Base.Infrastructure.Services.LoggerService;
using AppyNox.Services.Sso.Application;
using AppyNox.Services.Sso.Domain.Entities;
using AppyNox.Services.Sso.Infrastructure;
using AppyNox.Services.Sso.Infrastructure.Data;
using AppyNox.Services.Sso.Infrastructure.Localization;
using AppyNox.Services.Sso.WebAPI.ControllerDependencies;
using AppyNox.Services.Sso.WebAPI.Localization;
using AppyNox.Services.Sso.WebAPI.Middlewares;
using Asp.Versioning;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Serilog;
using System.Runtime.CompilerServices;
using System.Text.Json;

[assembly: InternalsVisibleTo("AppyNox.Services.Sso.WebAPI.UnitTest")]
[assembly: InternalsVisibleTo("AppyNox.Services.Sso.WebAPI.IntegrationTest")]
var builder = WebApplication.CreateBuilder(args);

#region [ Configuration Service ]

await builder.AddConsulConfiguration("SsoService");
var configuration = builder.Configuration;

#endregion

#region [ Logger Setup ]

builder.Host.UseSerilog((context, services, config) =>
    config.ReadFrom.Configuration(context.Configuration)
          .ReadFrom.Services(services)
          .Enrich.FromLogContext()
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
NoxLogger noxLogger = new(logger, "SsoHost");

#endregion

#endregion

#region [ Configure Services ]

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
}); ;

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Web API v1", Version = "version 1" });
});

builder.Services.AddHealthChecks();

#endregion

#region [ Dependency Injection For Layers ]

noxLogger.LogInformation("Registering DI's for layers.");
builder.Services.AddSsoApplication(configuration);
builder.Services.AddSsoInfrastructure(configuration, noxLogger);
noxLogger.LogInformation("Registering DI's for layers completed.");

#endregion

#region [ Dependency Injection Setup ]

builder.Services.AddScoped<UsersControllerBaseDependencies>();

#endregion

#region [ Localization Configuration ]

builder.ConfigureLocalization();

#endregion

var app = builder.Build();

#region [ Localization Services ]

IStringLocalizerFactory localizerFactory = app.Services.GetRequiredService<IStringLocalizerFactory>();
localizerFactory.AddNoxLocalizationServices();

NoxSsoInfrastructureResourceService.Initialize(localizerFactory);
NoxSsoApiResourceService.Initialize(localizerFactory);

#endregion

#region [ Pipeline ]

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRequestLocalization();

app.UseNoxContext();

app.UseNoxResponseWrapper(new NoxResponseWrapperOptions
{
    ApiVersion = NoxVersions.v1_0,
});

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<SsoContextMiddleware>();

app.MapControllers();

app.UseHealthChecks("/api/health");

#endregion

#region [ Hosted Services ]

var consulHostedService = app.Services.GetServices<IHostedService>()
    .OfType<ConsulHostedService>()
    .First();

consulHostedService.OnConsulConnectionFailed += (Exception ex) =>
{
    noxLogger.LogError(ex, "Consul connection failed. Stopping application.");
    var lifeTime = app.Services.GetService<IHostApplicationLifetime>();
    lifeTime?.StopApplication();
    return Task.CompletedTask;
};

#endregion

#region [ Migrations ]

app.Services.ApplyMigrations<IdentityDatabaseContext>();

app.Services.ApplyMigrations<IdentitySagaDatabaseContext>();

#endregion

app.Run();