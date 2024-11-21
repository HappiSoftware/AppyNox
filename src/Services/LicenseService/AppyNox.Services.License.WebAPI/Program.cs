using AppyNox.Services.Base.API;
using AppyNox.Services.Base.Infrastructure.Extensions;
using AppyNox.Services.License.Application;
using AppyNox.Services.License.Infrastructure;
using AppyNox.Services.License.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

await builder.AddApiServices(options =>
{
    options.SetupHostName = "CouponHost";
    options.UseConsulKV = true;
    options.ConfigureLayers = (logger, configuration) =>
    {
        builder
            .AddLicenseApplication(configuration, logger)
            .AddLicenseInfrastructure(configuration, logger);
    };
});

var app = builder.Build();

app.ConfigureNoxApi(options =>
{
    options.PerformAutoMapperChecks = false;
});

#region [ Migrations ]

app.Services.ApplyMigrations<LicenseDatabaseContext>();

#endregion

app.Run();