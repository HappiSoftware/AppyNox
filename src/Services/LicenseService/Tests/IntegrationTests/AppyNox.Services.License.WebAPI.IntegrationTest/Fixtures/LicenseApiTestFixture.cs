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
    public class LicenseApiTestFixture : DockerComposeTestBase
    {
        #region [ Properties ]

        public LicenseDatabaseContext DbContext { get; private set; }

        #endregion

        #region [ Public Constructors ]

        public LicenseApiTestFixture()
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
                    Services = ["appynox-consul", "appynox-gateway-ocelotgateway", "appynox-license-db",
                        "appynox-services-license-webapi", "appynox-authentication-db", "appynox-services-authentication-webapi"],
                });
        }

        #endregion
    }
}