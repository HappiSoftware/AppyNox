using AppyNox.Services.Base.Application.Interfaces.Encryption;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Application.Interfaces.Repositories;
using AppyNox.Services.Base.Core.Common;
using AppyNox.Services.Base.Infrastructure.BackgroundJobs;
using AppyNox.Services.Base.Infrastructure.Data.Interceptors;
using AppyNox.Services.Base.Infrastructure.HostedServices;
using AppyNox.Services.Base.Infrastructure.Services;
using AppyNox.Services.Base.Infrastructure.Services.LoggerService;
using AppyNox.Services.Coupon.Infrastructure.Data;
using AppyNox.Services.Coupon.Infrastructure.Repositories;
using Consul;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;

namespace AppyNox.Services.Coupon.Infrastructure
{
    public static class DependencyInjection
    {
        #region [ Public Methods ]

        /// <summary>
        /// Centralized Dependency Injection For Infrastructure Layer.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static IServiceCollection AddCouponInfrastructure(this IServiceCollection services, IConfiguration configuration, INoxLogger logger)
        {
            services.AddSingleton<INoxInfrastructureLogger, NoxInfrastructureLogger>();
            services.AddSingleton<INoxApplicationLogger, NoxApplicationLogger>();
            services.AddSingleton<INoxApiLogger, NoxApiLogger>();

            #region [ Database Configuration ]

            string? connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();

            services.AddDbContext<CouponDbContext>((sp, options) =>
            {
                ConvertDomainEventsToOutboxMessagesInterceptor outboxMessageInterceptor = sp.GetService<ConvertDomainEventsToOutboxMessagesInterceptor>() 
                    ?? throw new ArgumentException("ConvertDomainEventsToOutboxMessagesInterceptor was null!");

                options.UseNpgsql(connectionString)
                .AddInterceptors(outboxMessageInterceptor);
            });

            logger.LogInformation($"Connection String: {connectionString}");

            // Encryption
            services.AddSingleton<IEncryptionService, EncryptionService>();

            #endregion

            #region [ Consul Discovery Service ]

            services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
            {
                var address = configuration["ConsulConfiguration:Address"] ?? "http://localhost:8500";
                consulConfig.Address = new Uri(address);
            }));
            services.AddSingleton<IHostedService, ConsulHostedService>();
            services.Configure<ConsulConfiguration>(configuration.GetSection("consul"));

            #endregion

            #region [ Background Jobs ]

            services.AddQuartz(configure =>
            {
                var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob<CouponDbContext>));
                configure
                    .AddJob<ProcessOutboxMessagesJob<CouponDbContext>>(jobKey)
                    .AddTrigger(trigger => trigger.ForJob(jobKey).StartNow()
                    .WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(30).RepeatForever()));
            });

            services.AddQuartzHostedService();

            #endregion

            services.AddScoped(typeof(ICouponRepository), typeof(CouponRepository));
            services.AddScoped(typeof(INoxRepository<>), typeof(NoxRepository<>));
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        #endregion
    }
}