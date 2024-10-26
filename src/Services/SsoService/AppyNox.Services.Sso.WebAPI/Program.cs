using AppyNox.Services.Base.API;
using AppyNox.Services.Base.Infrastructure.Extensions;
using AppyNox.Services.Sso.Application;
using AppyNox.Services.Sso.Infrastructure;
using AppyNox.Services.Sso.Infrastructure.Data;
using AppyNox.Services.Sso.Infrastructure.Localization;
using AppyNox.Services.Sso.WebAPI.ControllerDependencies;
using AppyNox.Services.Sso.WebAPI.Localization;
using AppyNox.Services.Sso.WebAPI.Middlewares;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("AppyNox.Services.Sso.WebAPI.UnitTest")]
[assembly: InternalsVisibleTo("AppyNox.Services.Sso.WebAPI.IntegrationTest")]
var builder = WebApplication.CreateBuilder(args);

await builder.AddApiServices(options =>
{
    options.SetupHostName = "CouponHost";
    options.UseConsulKV = true;
    options.UseDynamicRequestBodyOperationFilter = false;
    options.ConfigureLayers = (services, logger, configuration) =>
    {
        services
            .AddSsoApplication(configuration, logger)
            .AddSsoInfrastructure(configuration, logger);
    };
});

#region [ Dependency Injection Setup ]

builder.Services.AddScoped<UsersControllerBaseDependencies>();

#endregion

var app = builder.Build();

app.ConfigureNoxApi(options =>
{
    options.LocalizationServices = (localizerFactory) =>
    {
        NoxSsoInfrastructureResourceService.Initialize(localizerFactory);
        NoxSsoApiResourceService.Initialize(localizerFactory);
    };
    options.PerformAutoMapperChecks = false;
});

app.UseMiddleware<SsoContextMiddleware>();

#region [ Migrations ]

app.Services.ApplyMigrations<IdentityDatabaseContext>();

app.Services.ApplyMigrations<IdentitySagaDatabaseContext>();

#endregion

await app.RunAsync();