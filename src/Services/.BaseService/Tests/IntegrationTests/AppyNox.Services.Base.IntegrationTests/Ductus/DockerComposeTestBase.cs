using AppyNox.Services.Base.Infrastructure.Services.LoggerService;
using AppyNox.Services.Base.IntegrationTests.Helpers;
using AppyNox.Services.Base.IntegrationTests.URIs;
using AutoWrapper.Server;
using AutoWrapper.Wrappers;
using Ductus.FluentDocker.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace AppyNox.Services.Base.IntegrationTests.Ductus
{
    /// <summary>
    /// Provides a base class for integration tests using Docker Compose.
    /// </summary>
    public abstract class DockerComposeTestBase : IDisposable
    {
        #region [ Fields ]

        public readonly JsonSerializerOptions JsonSerializerOptions;

        protected ICompositeService? CompositeService;

        protected IHostService? DockerHost;

        private bool _disposed;

        #endregion

        #region [ Properties ]

        public HttpClient Client { get; private set; }

        public string BearerToken { get; private set; } = string.Empty;

        public ServiceURIs ServiceURIs { get; private set; }

        protected NoxLogger Logger { get; private set; }

        #endregion

        #region [ Public Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="DockerComposeTestBase"/> class.
        /// </summary>
        protected DockerComposeTestBase()
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddSerilog();
            });
            var logger = loggerFactory.CreateLogger<INoxLogger>();
            Logger = new NoxLogger(logger, "DockerComposeTestBase");

            ServiceURIs = IntegrationTestHelpers.GetConfiguration("serviceuris").GetSection("ServiceUris").Get<ServiceURIs>()
                          ?? throw new InvalidOperationException("Service URIs configuration section is missing or invalid.");

            Client = new HttpClient { BaseAddress = new(ServiceURIs.GatewayURI) };

            JsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            EnsureDockerHost();
        }

        #endregion

        #region [ Public Methods ]

        /// <summary>
        /// Disposes resources used by the test, handling container teardown.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region [ Protected Methods ]

        /// <summary>
        /// Builds the composite service required for the test.
        /// </summary>
        /// <returns>The built composite service.</returns>
        protected abstract ICompositeService Build();

        /// <summary>
        /// Performs additional container teardown operations when disposing.
        /// </summary>
        protected virtual void OnContainerTearDown()
        {
        }

        /// <summary>
        /// Initializes the Docker Compose environment for testing.
        /// </summary>
        protected void Initialize(IConfigurationRoot configurationRoot)
        {
            #region [ Logger ]

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configurationRoot)
                .CreateLogger();

            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddSerilog();
            });
            var logger = loggerFactory.CreateLogger<INoxLogger>();
            Logger = new NoxLogger(logger, "CouponIntegrationTestHost");

            #endregion

            Logger.LogInformation("Initializing Docker Compose Test Base");
            CompositeService = Build();
            try
            {
                CompositeService.Start();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error starting docker compose");

                CompositeService.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Disposes of the resources used by the DockerComposeTestBase instance.
        /// </summary>
        /// <param name="disposing">Indicates whether the method is being called from the Dispose method (true) or from the finalizer (false).</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                OnContainerTearDown();
                CompositeService?.Dispose();
                DockerHost?.Dispose();
            }
            _disposed = true;
        }

        protected async Task WaitForServicesHealth(string healthUri, int maxAttempts = 10)
        {
            int attempts = 0;
            while (attempts < maxAttempts)
            {
                try
                {
                    Logger.LogInformation($"Checking service health at '{healthUri}'");
                    var response = await Client.GetAsync(healthUri);
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Logger.LogInformation($"Response: {responseContent}");
                    if (response.IsSuccessStatusCode)
                    {
                        return; // Service is healthy, exit the loop
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "Error while checking service health");
                }

                await Task.Delay(TimeSpan.FromSeconds(10));
                attempts++;
            }

            Logger.LogWarning($"Service did not become healthy in time '{healthUri}'");
            throw new Exception($"Service did not become healthy in time '{healthUri}'");
        }

        protected async Task AuthenticateAndGetToken()
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

        #region [ Destructor ]

        /// <summary>
        /// Finalizes the DockerComposeTestBase instance, ensuring proper resource cleanup.
        /// </summary>
        ~DockerComposeTestBase()
        {
            Dispose(false);
        }

        #endregion

        #region [ Private Methods ]

        private void EnsureDockerHost()
        {
            if (DockerHost?.State == ServiceRunningState.Running) return;

            var hosts = new Hosts().Discover();
            DockerHost = hosts.FirstOrDefault(x => x.IsNative) ?? hosts.FirstOrDefault(x => x.Name == "default");

            if (DockerHost != null)
            {
                if (DockerHost.State != ServiceRunningState.Running) DockerHost.Start();

                return;
            }

            if (hosts.Count == 0) DockerHost = hosts[0];

            if (DockerHost != null) return;

            EnsureDockerHost();
        }

        #endregion
    }
}