﻿using AppyNox.Services.Base.Infrastructure.Services;
using AppyNox.Services.Base.IntegrationTests.Ductus;
using AppyNox.Services.Base.IntegrationTests.Helpers;
using AppyNox.Services.Base.IntegrationTests.URIs;
using AppyNox.Services.Coupon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AppyNox.Services.Coupon.WebAPI.IntegrationTest.Fixtures;

public class CouponServiceFixture : DockerComposeTestBase
{
    #region [ Properties ]

    public CouponDbContext DbContext { get; private set; }

    #endregion

    #region [ Public Constructors ]

    public CouponServiceFixture()
    {
        IConfigurationRoot appsettings = IntegrationTestHelpers.GetConfiguration("appsettings.Staging");

        string[] services =
            [
                "appynox-rabbitmq-service",
                "appynox-common-rabbitmq-service",
                "appynox-consul",
                "appynox-gateway-ocelotgateway",
                "appynox-coupon-db",
                "appynox-redis",
                "appynox-services-coupon-webapi",
                "appynox-sso-db",
                "appynox-sso-saga-db",
                "appynox-services-sso-webapi"
            ];

        Initialize(appsettings, "CouponIntegrationTestHost", services);

        Task.WhenAll(
            WaitForServicesHealth(ServiceURIs.CouponServiceHealthURI),
            WaitForServicesHealth(ServiceURIs.SsoServiceHealthURI)
        ).GetAwaiter().GetResult();
        AuthenticateAndGetToken().GetAwaiter().GetResult();

        var options = new DbContextOptionsBuilder<CouponDbContext>()
            .UseNpgsql(appsettings.GetConnectionString("TestConnection"))
            .Options;

        EncryptionService encryptionService = new(appsettings);

        DbContext = new CouponDbContext(options, encryptionService);
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
}