using AppyNox.Services.Base.IntegrationTests.Ductus;
using AppyNox.Services.Base.IntegrationTests.Helpers;
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
using Microsoft.Extensions.Logging;
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

        private static readonly IConfiguration Configuration = IntegrationTestHelpers.GetConfiguration();

        private static readonly ILogger _logger = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        }).CreateLogger<CouponApiTestFixture>();

        private readonly ServiceCollection _services;

        private readonly IServiceProvider _serviceProvider;

        #endregion

        #region [ Properties ]

        public readonly JsonSerializerOptions JsonSerializerOptions;

        public CouponDbContext DbContext { get; private set; }

        public HttpClient Client { get; private set; }

        public string BearerToken { get; private set; } = string.Empty;

        public ServiceURIs ServiceURIs { get; private set; }

        #endregion

        #region [ Public Constructors ]

        public CouponApiTestFixture()
        {
            Initialize();
            _services = new ServiceCollection();
            _serviceProvider = ConfigureServices(_services);

            ServiceURIs = Configuration.GetSection("ServiceUris").Get<ServiceURIs>()
                          ?? throw new InvalidOperationException("Service URIs configuration section is missing or invalid.");

            Client = new HttpClient { BaseAddress = new(ServiceURIs.GatewayURI) };
            Task.WhenAll(
                WaitForServicesHealth(ServiceURIs.CouponServiceHealthURI),
                WaitForServicesHealth(ServiceURIs.AuthenticationServiceHealthURI)
            ).GetAwaiter().GetResult();
            AuthenticateAndGetToken().GetAwaiter().GetResult();

            DbContext = _serviceProvider.GetRequiredService<CouponDbContext>();
            JsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        #endregion

        #region [ Protected Methods ]

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Dispose managed resources specific to this class
                Client?.Dispose();
                DbContext?.Dispose();
            }

            // Call the base class's Dispose method
            base.Dispose(disposing);
        }

        protected override ICompositeService Build()
        {
            var file = Path.Combine(Directory.GetCurrentDirectory(), (TemplateString)"docker-compose.yml");
            var fileStaging = Path.Combine(Directory.GetCurrentDirectory(), (TemplateString)"docker-compose.staging.yml");

            return new DockerComposeCompositeService(DockerHost,
                new Ductus.FluentDocker.Model.Compose.DockerComposeConfig
                {
                    ComposeFilePath = new List<string> { file, fileStaging },
                    ForceRecreate = true,
                    RemoveOrphans = true,
                    StopOnDispose = true,
                    Services = ["appynox-consul", "appynox-gateway-ocelotgateway", "appynox-coupon-db",
                        "appynox-services-coupon-webapi", "appynox-authentication-db", "appynox-services-authentication-webapi"]
                });
        }

        #endregion

        #region [ Private Methods ]

        private static ServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Build the connection string from api appsettings.json
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.Staging.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
            var connectionString = config.GetConnectionString("TestConnection") ?? throw new InvalidOperationException("Connection string for CouponDb is missing.");

            // Add the DbContext with the connection string obtained
            services.AddDbContext<CouponDbContext>(options => options.UseNpgsql(connectionString));

            return services.BuildServiceProvider();
        }

        private async Task WaitForServicesHealth(string healthUri, int maxAttempts = 10)
        {
            int attempts = 0;
            while (attempts < maxAttempts)
            {
                try
                {
                    _logger.LogInformation("{Message}", $"Checking service health at '{healthUri}'");
                    var response = await Client.GetAsync(healthUri);
                    var responseContent = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation("{Message}", $"Response: {responseContent}");
                    if (response.IsSuccessStatusCode)
                    {
                        return; // Service is healthy, exit the loop
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while checking service health");
                }

                await Task.Delay(TimeSpan.FromSeconds(10));
                attempts++;
            }

            _logger.LogError("{Message}", $"Service did not become healthy in time '{healthUri}'");
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