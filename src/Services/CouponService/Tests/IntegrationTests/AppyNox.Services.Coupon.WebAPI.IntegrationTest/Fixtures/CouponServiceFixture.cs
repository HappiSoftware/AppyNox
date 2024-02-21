﻿using AppyNox.Services.Base.IntegrationTests.Ductus;
using AppyNox.Services.Base.IntegrationTests.Helpers;
using AppyNox.Services.Base.IntegrationTests.URIs;
using AppyNox.Services.Coupon.Infrastructure.Data;
using Ductus.FluentDocker.Model.Common;
using Ductus.FluentDocker.Services;
using Ductus.FluentDocker.Services.Impl;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AppyNox.Services.Coupon.WebAPI.IntegrationTest.Fixtures
{
    public class CouponServiceFixture : DockerComposeTestBase
    {
        #region [ Properties ]

        public CouponDbContext DbContext { get; private set; }

        #endregion

        #region [ Public Constructors ]

        public CouponServiceFixture()
        {
            IConfigurationRoot appsettings = IntegrationTestHelpers.GetConfiguration("appsettings.Staging");

            Initialize(appsettings, "CouponIntegrationTestHost");

            Task.WhenAll(
                WaitForServicesHealth(ServiceURIs.CouponServiceHealthURI),
                WaitForServicesHealth(ServiceURIs.SsoServiceHealthURI)
            ).GetAwaiter().GetResult();
            AuthenticateAndGetToken().GetAwaiter().GetResult();

            var options = new DbContextOptionsBuilder<CouponDbContext>()
                .UseNpgsql(appsettings.GetConnectionString("TestConnection"))
                .Options;

            DbContext = new CouponDbContext(options);
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
                        "appynox-coupon-db",
                        "appynox-redis",
                        "appynox-services-coupon-webapi",
                        "appynox-sso-db",
                        "appynox-sso-saga-db",
                        "appynox-services-sso-webapi",
                        "appynox-redis"
                    ]
                });
        }

        #endregion
    }
}