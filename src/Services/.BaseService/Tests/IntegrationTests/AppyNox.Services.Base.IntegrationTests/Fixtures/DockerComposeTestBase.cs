using AppyNox.Services.Base.Infrastructure.Services.LoggerService;
using AppyNox.Services.Base.IntegrationTests.Helpers;
using AppyNox.Services.Base.IntegrationTests.URIs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace AppyNox.Services.Base.IntegrationTests.Fixtures;

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

    private string RootDirectory { get; set; } = string.Empty;

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

        Client = new HttpClient()
        {
            BaseAddress = new Uri(ServiceURIs.GatewayURI)
        };

        JsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
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

        Logger.LogInformation("Initializing Docker Compose Test Base", false);

        string currentDirectory = Directory.GetCurrentDirectory();
        string targetDirectoryName = "AppyNox";

        while (true)
        {
            var directoryInfo = new DirectoryInfo(currentDirectory);

            if (directoryInfo.Name.Equals(targetDirectoryName, StringComparison.OrdinalIgnoreCase))
            {
                break; // We've reached the target directory
            }

            if (directoryInfo.Parent == null)
            {
                throw new InvalidOperationException($"Target directory '{targetDirectoryName}' not found in the path '{Directory.GetCurrentDirectory()}'");
            }

            currentDirectory = directoryInfo.Parent.FullName;
        }
        RootDirectory = Path.GetFullPath(currentDirectory);
        Logger.LogInformation($"Resolved RootDirectory: {RootDirectory}");

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

    protected static async Task IsDatabaseHealthy(DbContext context, int maxAttempts = 10, int delayInSeconds = 5)
    {
        int attempts = 0;
        while (attempts < maxAttempts)
        {
            try
            {
                var canConnect = await context.Database.CanConnectAsync();
                if (canConnect) return;
            }
            catch (Exception)
            {
            }

            await Task.Delay(TimeSpan.FromSeconds(delayInSeconds));
            attempts++;
        }

        throw new Exception("Database did not get healthy.");
    }

    #endregion

    #region [ Destructor ]

    /// <summary>
    /// Disposes of the resources used by the DockerComposeTestBase instance.
    /// </summary>
    public virtual void Dispose()
    {
        if (!_disposed)
        {
            StopDockerCompose();
            Client?.Dispose(); // Assuming Client is a managed resource
            _disposed = true;
            GC.SuppressFinalize(this);
        }
    }

    #endregion

    #region [ Private Methods ]

    private void StartDockerCompose(string[] services)
    {
        string serviceList = string.Join(" ", services);
        ExecuteShellCommand("docker", $"compose up -d {serviceList}", RootDirectory);
    }

    private void StopDockerCompose()
    {
        ExecuteShellCommand("docker", "compose down", RootDirectory);
    }

    private void ExecuteShellCommand(string command, string arguments, string workingDirectory = "", int timeoutInSeconds = 60)
    {
        Logger.LogInformation($"Executing command: {command} {arguments} in directory: {workingDirectory}");

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = command,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = workingDirectory
            }
        };

        var outputBuilder = new StringBuilder();
        var errorBuilder = new StringBuilder();

        process.OutputDataReceived += (sender, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                outputBuilder.AppendLine(e.Data);
                Logger.LogInformation(e.Data); // Log the output in real-time
            }
        };

        process.ErrorDataReceived += (sender, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                errorBuilder.AppendLine(e.Data);
                Logger.LogWarning(e.Data); // Log the error in real-time
            }
        };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        // Wait for the process to exit or timeout
        bool exited = process.WaitForExit(timeoutInSeconds * 1000);

        if (!exited)
        {
            TimeoutException exception = new($"The command '{command} {arguments}' timed out after {timeoutInSeconds} seconds.");
            // Kill the process if it did not exit in time
            process.Kill();
            Logger.LogError(exception, $"The command '{command} {arguments}' timed out after {timeoutInSeconds} seconds.");
            Logger.LogError(exception, "Captured Output:");
            Logger.LogError(exception, outputBuilder.ToString()); // Log the captured output
            Logger.LogError(exception, "Captured Errors:");
            Logger.LogError(exception, errorBuilder.ToString()); // Log the captured errors
            throw exception;
        }

        // Log remaining output and error after process exits
        Logger.LogInformation(outputBuilder.ToString());
        if (errorBuilder.Length > 0)
        {
            Logger.LogWarning(errorBuilder.ToString());
        }

        if (process.ExitCode != 0)
        {
            throw new Exception($"Command '{command} {arguments}' failed with error: {errorBuilder.ToString()}");
        }
    }

    #endregion
}
