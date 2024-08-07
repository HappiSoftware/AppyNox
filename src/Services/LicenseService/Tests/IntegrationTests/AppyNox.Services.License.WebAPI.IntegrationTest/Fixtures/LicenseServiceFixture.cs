using AppyNox.Services.Base.IntegrationTests.Ductus;
using AppyNox.Services.Base.IntegrationTests.Helpers;
using AppyNox.Services.Base.IntegrationTests.URIs;
using AppyNox.Services.License.Infrastructure.Data;
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

            string[] services =
                [
                    "appynox-rabbitmq-service",
                    "appynox-common-rabbitmq-service",
                    "appynox-consul",
                    "appynox-gateway-ocelotgateway",
                    "appynox-license-db",
                    "appynox-redis",
                    "appynox-services-license-webapi",
                    "appynox-sso-db",
                    "appynox-sso-saga-db",
                    "appynox-services-sso-webapi"
                ];

            Initialize(appsettings, "LicenseIntegrationTestHost", services);

            Task.WhenAll(
                WaitForServicesHealth(ServiceURIs.SsoServiceHealthURI),
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