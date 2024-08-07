using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Core.Enums;
using AppyNox.Services.Base.Infrastructure.Services.LoggerService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using AppyNox.Services.Coupon.Infrastructure;
using AppyNox.Services.Base.Application.Interfaces.Caches;
using AppyNox.Services.Base.Infrastructure.Services.CacheServices;
using StackExchange.Redis;
using AppyNox.Services.Base.Infrastructure.Configuration;
using Microsoft.AspNetCore.Builder;

namespace AppyNox.Services.Coupon.Benchmark;

internal static class DependencyInjection
{
    internal static void AddInfrastructureBenchmark(this IServiceCollection serviceCollection)
    {
        var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.Development.json", optional: true)
                    .AddEnvironmentVariables()
                    .Build();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddSerilog();
        });
        var logger = loggerFactory.CreateLogger<ApplicationBuilder>();
        NoxLogger<ApplicationBuilder> noxLogger = new(logger, "CouponBenchmark");

        serviceCollection.AddLogging(builder =>
            builder.AddConsole()
                   .AddDebug());

        RedisConfiguration? redisConfig = configuration.GetSection("Redis").Get<RedisConfiguration>();
        if (redisConfig == null || string.IsNullOrWhiteSpace(redisConfig.ConnectionString))
        {
            throw new InvalidOperationException("Redis configuration is missing or invalid.");
        }

        serviceCollection.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConfig.ConnectionString));
        serviceCollection.AddSingleton<ICacheService, RedisCacheService>();

        serviceCollection.AddCouponInfrastructure(configuration, noxLogger);
    }
}