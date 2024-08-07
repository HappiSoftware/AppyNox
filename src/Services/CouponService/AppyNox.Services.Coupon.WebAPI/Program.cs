using AppyNox.Services.Base.API;
using AppyNox.Services.Base.Infrastructure.Extensions;
using AppyNox.Services.Coupon.Application;
using AppyNox.Services.Coupon.Domain;
using AppyNox.Services.Coupon.Infrastructure;
using AppyNox.Services.Coupon.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

await builder.AddApiServices(options =>
{
    options.SetupHostName = "CouponHost";
    options.UseConsulKV = true;
    options.ConfigureLayers = (services, logger, configuration) =>
    {
        services
            .AddCouponApplication(configuration, logger)
            .AddCouponInfrastructure(configuration, logger);
    };
});

var app = builder.Build();

app.ConfigureNoxApi(options =>
{
    options.LocalizationServices = (localizerFactory) =>
    {
        localizerFactory.AddCouponDomainLocalizationService();
    };
    options.PerformAutoMapperChecks = false;
});

app.Services.ApplyMigrations<CouponDbContext>();

app.Run();