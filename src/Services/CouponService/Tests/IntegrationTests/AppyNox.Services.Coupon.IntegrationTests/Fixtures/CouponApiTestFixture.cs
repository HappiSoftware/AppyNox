using AppyNox.Services.Base.IntegrationTests.Ductus;
using AppyNox.Services.Coupon.Infrastructure.Data;
using Ductus.FluentDocker.Commands;
using Ductus.FluentDocker.Model.Common;
using Ductus.FluentDocker.Model.Containers;
using Ductus.FluentDocker.Services;
using Ductus.FluentDocker.Services.Impl;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AppyNox.Services.Coupon.WebAPI.IntegrationTests.Fixtures
{
    public class CouponApiTestFixture : DockerComposeTestBase
    {
        #region [ Fields ]

        private readonly ServiceCollection _services;

        #endregion

        #region [ Properties ]

        public readonly string CouponURI = "https://localhost:7002";

        public CouponDbContext DbContext { get; private set; }

        public HttpClient Client { get; private set; }

        #endregion

        #region [ Public Constructors ]

        public CouponApiTestFixture()
        {
            _services = new ServiceCollection();
            ConfigureServices(_services);
            var serviceProvider = _services.BuildServiceProvider();
            DbContext = serviceProvider.GetRequiredService<CouponDbContext>();

            Client = new HttpClient { BaseAddress = new(CouponURI) };

            AppyNox.Services.Coupon.Infrastructure.DependencyInjection.ApplyMigrations(serviceProvider);

            WaitForServiceHealth($"{CouponURI}/health-check", 60).GetAwaiter().GetResult();
        }

        #endregion

        #region [ Protected Methods ]

        protected override ICompositeService Build()
        {
            var file = Path.Combine(Directory.GetCurrentDirectory(), (TemplateString)"docker-compose.yml");

            return new DockerComposeCompositeService(DockerHost,
                new Ductus.FluentDocker.Model.Compose.DockerComposeConfig
                {
                    ComposeFilePath = new List<string> { file },
                    ForceRecreate = true,
                    RemoveOrphans = true,
                    StopOnDispose = true
                });
        }

        #endregion

        #region [ Private Methods ]

        private async Task WaitForServiceHealth(string healthCheckUrl, int timeoutSeconds)
        {
            var startTime = DateTime.UtcNow;
            while (DateTime.UtcNow - startTime < TimeSpan.FromSeconds(timeoutSeconds))
            {
                try
                {
                    var response = await Client.GetAsync(healthCheckUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        return; // Service is healthy, exit the loop
                    }
                }
                catch
                {
                    // The request failed, possibly because the service is not up yet. Ignore and retry.
                }

                await Task.Delay(1000); // Wait for a second before retrying
            }

            throw new Exception("Service did not become healthy in time");
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            // Build the connection string from api appsettings.json
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), @"../../../../../../AppyNox.Services.Coupon.WebAPI"))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.Staging.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
            var connectionString = config.GetConnectionString("TestConnection") ?? throw new InvalidOperationException("Connection string for CouponDb is missing.");

            // Add the DbContext with the connection string obtained
            services.AddDbContext<CouponDbContext>(options => options.UseNpgsql(connectionString));
        }

        #endregion
    }
}