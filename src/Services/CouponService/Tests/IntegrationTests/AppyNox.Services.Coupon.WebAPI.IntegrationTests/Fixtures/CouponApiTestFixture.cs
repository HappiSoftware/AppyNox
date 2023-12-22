using AppyNox.Services.Base.IntegrationTests.Ductus;
using AppyNox.Services.Base.IntegrationTests.URIs;
using AppyNox.Services.Coupon.Infrastructure.Data;
using AutoWrapper.Server;
using AutoWrapper.Wrappers;
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
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace AppyNox.Services.Coupon.WebAPI.IntegrationTests.Fixtures
{
    public class CouponApiTestFixture : DockerComposeTestBase
    {
        #region [ Fields ]

        private readonly ServiceCollection _services;

        #endregion

        #region [ Properties ]

        public readonly JsonSerializerOptions JsonSerializerOptions;

        public CouponDbContext DbContext { get; private set; }

        public HttpClient Client { get; private set; }

        public string BearerToken { get; private set; } = string.Empty;

        #endregion

        #region [ Public Constructors ]

        public CouponApiTestFixture()
        {
            Client = new HttpClient { BaseAddress = new(ServiceURIs.GatewayURI) };
            Task.WhenAll(
                WaitForServicesHealth(ServiceURIs.CouponServiceHealthURI),
                WaitForServicesHealth(ServiceURIs.AuthenticationServiceHealthURI)
            ).GetAwaiter().GetResult();
            AuthenticateAndGetToken().GetAwaiter().GetResult();

            _services = new ServiceCollection();
            ConfigureServices(_services);
            var serviceProvider = _services.BuildServiceProvider();
            DbContext = serviceProvider.GetRequiredService<CouponDbContext>();
            JsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
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

        private async Task WaitForServicesHealth(string healthUri, int maxAttempts = 10)
        {
            int attempts = 0;
            while (attempts < maxAttempts)
            {
                try
                {
                    var response = await Client.GetAsync(healthUri);
                    if (response.IsSuccessStatusCode)
                    {
                        return; // Service is healthy, exit the loop
                    }
                }
                catch
                {
                    // The request failed, possibly because the service is not up yet. Ignore and retry.
                }

                await Task.Delay(TimeSpan.FromSeconds(5));
                attempts++;
            }

            throw new Exception($"Service did not become healthy in time '{healthUri}'");
        }

        private async Task AuthenticateAndGetToken()
        {
            var authUri = ServiceURIs.AuthenticationServiceURI + "/authentication/connect/token";
            var content = new StringContent(
                JsonSerializer.Serialize(new { userName = "admin", password = "Admin@123" }),
                Encoding.UTF8,
                "application/json"
            );

            var response = await Client.PostAsync(authUri, content);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var apiResponse = Unwrapper.Unwrap<ApiResponse>(responseString);

            if (apiResponse?.Result is JsonElement resultElement &&
                resultElement.TryGetProperty("token", out JsonElement tokenElement))
            {
                BearerToken = tokenElement.GetString() ?? throw new Exception("Token was null");
            }
            else
            {
                throw new Exception("Authentication failed or token was not found in the response.");
            }

            Client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", BearerToken);
        }

        #endregion
    }
}