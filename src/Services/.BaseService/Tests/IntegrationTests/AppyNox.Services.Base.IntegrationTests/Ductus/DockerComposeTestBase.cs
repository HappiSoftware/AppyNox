using Ductus.FluentDocker.Services;
using Microsoft.Extensions.Hosting;

namespace AppyNox.Services.Base.IntegrationTests.Ductus
{
    public abstract class DockerComposeTestBase : IDisposable
    {
        #region Fields

        protected ICompositeService? CompositeService;

        protected IHostService? DockerHost;

        private bool _disposed;

        #endregion

        #region Public Constructors

        protected DockerComposeTestBase()
        {
            EnsureDockerHost();
        }

        #endregion

        #region Public Methods

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Protected Methods

        protected abstract ICompositeService Build();

        protected virtual void OnContainerTearDown()
        {
        }

        protected void Initialize()
        {
            CompositeService = Build();
            try
            {
                CompositeService.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                CompositeService.Dispose();
                throw;
            }
        }

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

        #region Destructor

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