using AppyNox.Services.Base.IntegrationTests.Ductus;
using AppyNox.Services.Base.IntegrationTests.Helpers;
using AppyNox.Services.Base.IntegrationTests.URIs;
using AppyNox.Services.License.Infrastructure.Data;
using Ductus.FluentDocker.Model.Common;
using Ductus.FluentDocker.Services;
using Ductus.FluentDocker.Services.Impl;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AppyNox.Services.License.WebAPI.IntegrationTest.Fixtures
{
    public class LicenseServiceFixture : DockerComposeTestBase
    {
        #region [ Properties ]

        public LicenseDatabaseContext DbContext { get; private set; }

        #endregion

        #region [ Public Constructors ]

        public LicenseServiceFixture()
        {
            IConfigurationRoot appsettings = IntegrationTestHelpers.GetConfiguration("appsettings.Staging");

            Initialize(appsettings, "LicenseIntegrationTestHost");

            Task.WhenAll(
                WaitForServicesHealth(ServiceURIs.AuthenticationServiceHealthURI),
                WaitForServicesHealth(ServiceURIs.LicenseServiceHealthURI)
            ).GetAwaiter().GetResult();
            AuthenticateAndGetToken().GetAwaiter().GetResult();

            var options = new DbContextOptionsBuilder<LicenseDatabaseContext>()
                .UseNpgsql(appsettings.GetConnectionString("TestConnection"))
                .Options;

            DbContext = new LicenseDatabaseContext(options);
            IsDatabaseHealthy().GetAwaiter().GetResult();
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
            var fileStaging = Path.Combine(Directory.GetCurrentDirectory(), (TemplateString)"docker-compose.Staging.yml");

            return new DockerComposeCompositeService(DockerHost,
                new Ductus.FluentDocker.Model.Compose.DockerComposeConfig
                {
                    ComposeFilePath = new List<string> { file, fileStaging },
                    ForceRecreate = true,
                    RemoveOrphans = true,
                    StopOnDispose = true,
                    Services =
                    [
                        "appynox-rabbitmq-service",
                        "appynox-consul",
                        "appynox-gateway-ocelotgateway",
                        "appynox-license-db",
                        "appynox-services-license-webapi",
                        "appynox-authentication-db",
                        "appynox-services-authentication-webapi"
                    ],
                });
        }

        #endregion

        #region [ Private Methods ]

        private async Task IsDatabaseHealthy(int maxAttempts = 10, int delayInSeconds = 5)
        {
            int attempts = 0;
            while (attempts < maxAttempts)
            {
                try
                {
                    // Try a simple operation, like checking if a table exists
                    var canConnect = await DbContext.Database.CanConnectAsync();
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
    }
}