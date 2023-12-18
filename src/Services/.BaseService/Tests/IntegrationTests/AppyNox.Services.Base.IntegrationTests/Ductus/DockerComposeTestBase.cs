using Ductus.FluentDocker.Services;
using Microsoft.Extensions.Hosting;

namespace AppyNox.Services.Base.IntegrationTests.Ductus
{
    public abstract class DockerComposeTestBase : IDisposable
    {
        #region Fields

        protected ICompositeService CompositeService;

        protected IHostService? DockerHost;

        #endregion

        #region Public Constructors

        public DockerComposeTestBase()
        {
            EnsureDockerHost();

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

        #endregion

        #region Public Methods

        public void Dispose()
        {
            OnContainerTearDown();
            var compositeService = CompositeService;
            CompositeService = null!;
            try
            {
                compositeService?.Dispose();
            }
            catch
            {
                // ignored
            }
        }

        #endregion

        #region Protected Methods

        protected abstract ICompositeService Build();

        protected virtual void OnContainerTearDown()
        {
        }

        #endregion

        #region Private Methods

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

            if (hosts.Count == 0) DockerHost = hosts.First();

            if (DockerHost != null) return;

            EnsureDockerHost();
        }

        #endregion
    }
}