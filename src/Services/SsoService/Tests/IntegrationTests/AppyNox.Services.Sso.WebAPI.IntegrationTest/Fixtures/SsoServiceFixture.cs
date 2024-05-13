using AppyNox.Services.Base.IntegrationTests.Ductus;
using AppyNox.Services.Base.IntegrationTests.Helpers;
using AppyNox.Services.Sso.Infrastructure.Data;
using Ductus.FluentDocker.Model.Common;
using Ductus.FluentDocker.Services;
using Ductus.FluentDocker.Services.Impl;
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

        Initialize(appsettings, "SsoIntegrationTestHost");

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

    protected override ICompositeService Build()
    {
        var file = Path.Combine(Directory.GetCurrentDirectory(), (TemplateString)"docker-compose.yml");
        var fileStaging = Path.Combine(Directory.GetCurrentDirectory(), (TemplateString)"docker-compose.Staging.yml");

        return new DockerComposeCompositeService(DockerHost,
            new Ductus.FluentDocker.Model.Compose.DockerComposeConfig
            {
                ComposeFilePath = [file, fileStaging],
                ForceRecreate = true,
                RemoveOrphans = true,
                StopOnDispose = true,
                Services =
                [
                    "appynox-rabbitmq-service",
                        "appynox-consul",
                        "appynox-gateway-ocelotgateway",
                        "appynox-sso-db",
                        "appynox-sso-saga-db",
                        "appynox-redis",
                        "appynox-services-sso-webapi"
                ],
            });
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