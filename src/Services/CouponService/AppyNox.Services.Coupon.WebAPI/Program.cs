using AppyNox.Services.Base.API.Middleware;
using AppyNox.Services.Base.Domain.Common;
using AppyNox.Services.Coupon.WebAPI.Helpers;
using AutoWrapper;
using Consul;
using NLog;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

#region [ SSL Configuration ]

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    string fileName = string.Empty;

    if (builder.Environment.IsDevelopment())
    {
        fileName = Directory.GetCurrentDirectory() + "/ssl/coupon-service.pfx";
    }
    else if (builder.Environment.IsProduction())
    {
        fileName = "/https/coupon-service.pfx";
    }

    serverOptions.ConfigureEndpointDefaults(listenOptions =>
    {
        listenOptions.UseHttps(fileName ?? throw new InvalidOperationException("SSL certificate file path could not be determined."), "happi2023");
    });
});

#endregion

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region [ Logger Setup ]

if (!builder.Environment.IsDevelopment())
{
    NLog.LogManager.Setup().LoadConfigurationFromFile("nlog.config");
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();
}

#endregion

#region [ Consul Discovery Service ]

builder.Services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
{
    var address = configuration["ConsulConfig:Address"] ?? "http://localhost:8500";
    consulConfig.Address = new Uri(address);
}));
builder.Services.AddSingleton<IHostedService, ConsulHostedService>();
builder.Services.Configure<ConsulConfig>(configuration.GetSection("consul"));

#endregion

builder.Services.AddHealthChecks();

AppyNox.Services.Coupon.Infrastructure.DependencyInjection.ConfigureServices(builder.Services, configuration);
AppyNox.Services.Coupon.Application.DependencyInjection.ConfigureServices(builder.Services, configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseApiResponseAndExceptionWrapper(new AutoWrapperOptions { IsApiOnly = true, ShowApiVersion = true, ApiVersion = "1.0" });
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<QueryParameterValidateMiddleware>();

AppyNox.Services.Coupon.Infrastructure.DependencyInjection.ApplyMigrations(app.Services);

app.UseHealthChecks("/health-check");

app.Run();