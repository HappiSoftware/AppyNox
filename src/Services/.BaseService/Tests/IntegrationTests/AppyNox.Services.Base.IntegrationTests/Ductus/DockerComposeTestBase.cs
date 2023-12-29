using Ductus.FluentDocker.Services;
using Microsoft.Extensions.Logging;

namespace AppyNox.Services.Base.IntegrationTests.Ductus
{
    /// <summary>
    /// Provides a base class for integration tests using Docker Compose.
    /// </summary>
    public abstract class DockerComposeTestBase : IDisposable
    {
        #region [ Fields ]

        protected ICompositeService? CompositeService;

        protected IHostService? DockerHost;

        private static readonly ILogger _logger = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        }).CreateLogger<DockerComposeTestBase>();

        private bool _disposed;

        #endregion

        #region [ Public Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="DockerComposeTestBase"/> class.
        /// </summary>
        protected DockerComposeTestBase()
        {
            EnsureDockerHost();
        }

        #endregion

        #region Public Methods

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
        protected void Initialize()
        {
            _logger.LogInformation("{Message}", "Initializing Docker Compose Test Base");
            CompositeService = Build();
            try
            {
                CompositeService.Start();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error starting docker compose");

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