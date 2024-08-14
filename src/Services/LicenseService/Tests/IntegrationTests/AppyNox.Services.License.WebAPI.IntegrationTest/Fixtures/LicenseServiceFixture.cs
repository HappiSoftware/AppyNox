using AppyNox.Services.Base.IntegrationTests.Fixtures;
using AppyNox.Services.Base.IntegrationTests.Helpers;
using AppyNox.Services.Base.IntegrationTests.URIs;
using AppyNox.Services.License.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AppyNox.Services.License.WebAPI.IntegrationTest.Fixtures;

public class LicenseServiceFixture : DockerComposeTestBase, IDisposable
{
    #region [ Properties ]

    public LicenseDatabaseContext DbContext { get; private set; }

    #endregion

    #region [ Public Constructors ]

    public LicenseServiceFixture()
        : base()
    {
        try
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
            IsDatabaseHealthy(DbContext).GetAwaiter().GetResult();
        }
        catch (Exception)
        {
            Dispose();
            throw;
        }
    }

    #endregion

    #region [ IDisposable ]

    public override void Dispose()
    {
        // Dispose managed resources specific to this class
        Client?.Dispose();
        DbContext?.Dispose();

        // Call the base class's Dispose method
        GC.SuppressFinalize(this);
        base.Dispose();
    }

    #endregion

}