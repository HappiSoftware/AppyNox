using AppyNox.Services.Base.IntegrationTests.Ductus;
using AppyNox.Services.Base.IntegrationTests.Helpers;
using AppyNox.Services.Sso.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AppyNox.Services.Sso.WebAPI.IntegrationTest.Fixtures;

public class SsoServiceFixture : DockerComposeTestBase
{
    #region [ Properties ]

    public IdentityDatabaseContext DbContext { get; private set; }

    public IdentitySagaDatabaseContext SagaDbContext { get; private set; }

    public IConfigurationRoot Configuration { get; private set; }

    #endregion

    #region [ Public Constructors ]

    public SsoServiceFixture()
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

        WaitForServicesHealth(ServiceURIs.SsoServiceHealthURI).GetAwaiter().GetResult();
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

    private static async Task IsDatabaseHealthy(DbContext context, int maxAttempts = 10, int delayInSeconds = 5)
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
}