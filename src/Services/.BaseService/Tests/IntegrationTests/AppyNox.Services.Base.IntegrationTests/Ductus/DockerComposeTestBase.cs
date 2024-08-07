using AppyNox.Services.Base.Infrastructure.Services.LoggerService;
using AppyNox.Services.Base.IntegrationTests.Helpers;
using AppyNox.Services.Base.IntegrationTests.URIs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace AppyNox.Services.Base.IntegrationTests.Ductus;

/// <summary>
/// Provides a base class for integration tests using Docker Compose.
/// </summary>
public abstract class DockerComposeTestBase : IDisposable
{
    #region [ Fields ]

    public readonly JsonSerializerOptions JsonSerializerOptions;
    private bool _disposed;

    #endregion

    #region [ Properties ]

    public HttpClient Client { get; private set; }
    public string BearerToken { get; private set; } = string.Empty;
    public ServiceURIs ServiceURIs { get; private set; }
    protected NoxLogger<DockerComposeTestBase> Logger { get; private set; }

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
        var logger = loggerFactory.CreateLogger<DockerComposeTestBase>();
        Logger = new(logger, "DockerComposeTestBase");

        ServiceURIs =
            IntegrationTestHelpers
                .GetConfiguration("serviceuris")
                .GetSection("ServiceUris")
                .Get<ServiceURIs>()
            ?? throw new InvalidOperationException(
                "Service URIs configuration section is missing or invalid."
            );

        Client = new HttpClient { BaseAddress = new(ServiceURIs.GatewayURI) };

        JsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
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
    /// Initializes the Docker Compose environment for testing.
    /// </summary>
    protected void Initialize(IConfigurationRoot configurationRoot, string layerName, string[] services)
    {
        #region [ Logger ]

        Log.Logger = new LoggerConfiguration().ReadFrom
            .Configuration(configurationRoot)
            .CreateLogger();

        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddSerilog();
        });
        var logger = loggerFactory.CreateLogger<DockerComposeTestBase>();
        Logger = new(logger, layerName);

        #endregion

        Logger.LogInformation("Initializing Docker Compose Test Base");

        try
        {
            StartDockerCompose(services);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error starting Docker Compose");
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
            StopDockerCompose();
            Client?.Dispose();
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

    protected async Task AuthenticateAndGetToken(
        string userName = "admin",
        string password = "Admin@123",
        string audience = "AppyNox",
        string ssoEndpoint = "/v1.0/authentication/connect/token"
    )
    {
        var ssoUri = ServiceURIs.SsoServiceURI + ssoEndpoint;
        var content = new StringContent(
            JsonSerializer.Serialize(
                new
                {
                    userName,
                    password,
                    audience
                }
            ),
            Encoding.UTF8,
            "application/json"
        );

        var response = await Client.PostAsync(ssoUri, content);
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();

        using (var document = JsonDocument.Parse(responseString))
        {
            var root = document.RootElement;
            var resultElement = root.GetProperty("result");
            var dataElement = resultElement.GetProperty("data");
            var token = dataElement.GetProperty("token").GetString();

            if (string.IsNullOrEmpty(token))
            {
                throw new Exception("Token was null or empty.");
            }

            BearerToken = token;
        }

        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer",
            BearerToken
        );
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

    private void BuildDockerCompose()
    {
        ExecuteShellCommand("docker", "compose -f docker-compose.yml -f docker-compose.Staging.yml build");
    }

    private void StartDockerCompose(string[] services)
    {
        string serviceList = string.Join(" ", services);
        Logger.LogInformation($"Starting Docker Compose services: {serviceList}");
        ExecuteShellCommand("docker", $"compose -f docker-compose.yml -f docker-compose.Staging.yml up -d {serviceList}");
    }


    private void StopDockerCompose()
    {
        ExecuteShellCommand("docker", "compose down");
    }

    private void ExecuteShellCommand(string command, string arguments)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = command,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        var output = new StringBuilder();
        var error = new StringBuilder();

        process.OutputDataReceived += (sender, args) => output.AppendLine(args.Data);
        process.ErrorDataReceived += (sender, args) => error.AppendLine(args.Data);

        process.Start();

        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        process.WaitForExit();

        // Ensure asynchronous read operations are complete
        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            throw new Exception($"Command '{command} {arguments}' failed with error: {error}");
        }

        Logger.LogInformation(output.ToString());
    }


    #endregion
}
