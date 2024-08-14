using AppyNox.Services.Base.IntegrationTests.Fixtures;
using AppyNox.Services.Base.IntegrationTests.Helpers;
using AppyNox.Services.Sso.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AppyNox.Services.Sso.WebAPI.IntegrationTest.Fixtures;

public class SsoServiceFixture : DockerComposeTestBase, IDisposable
{
    #region [ Properties ]

    public IdentityDatabaseContext DbContext { get; private set; }

    public IdentitySagaDatabaseContext SagaDbContext { get; private set; }

    public IConfigurationRoot Configuration { get; private set; }

    #endregion

    #region [ Public Constructors ]

    public SsoServiceFixture() 
        : base()
    {
        try
        {
            IConfigurationRoot appsettings = IntegrationTestHelpers.GetConfiguration("appsettings.Staging");
            Configuration = appsettings;
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

            Initialize(appsettings, "SsoIntegrationTestHost", services);

            Task.WhenAll(
                    WaitForServicesHealth(ServiceURIs.GatewayHealthURI),
                    WaitForServicesHealth(ServiceURIs.SsoServiceHealthURI)
                ).GetAwaiter().GetResult();
            AuthenticateAndGetToken().GetAwaiter().GetResult();

            DbContextOptions<IdentityDatabaseContext> options = new DbContextOptionsBuilder<IdentityDatabaseContext>()
                .UseNpgsql(appsettings.GetConnectionString("TestConnection"))
                .Options;

            DbContextOptions<IdentitySagaDatabaseContext> sagaOptions = new DbContextOptionsBuilder<IdentitySagaDatabaseContext>()
                .UseNpgsql(appsettings.GetConnectionString("TestConnection"))
                .Options;

            DbContext = new IdentityDatabaseContext(options);
            SagaDbContext = new IdentitySagaDatabaseContext(sagaOptions);

            Task.WhenAll(
                    IsDatabaseHealthy(DbContext),
                    IsDatabaseHealthy(SagaDbContext)
                ).GetAwaiter().GetResult();
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