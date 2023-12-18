using AppyNox.Services.Coupon.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Coupon.WebAPI.IntegrationTests.WebApiFactories
{
    internal class WebApiFactory : WebApplicationFactory<Program>
    {
        #region Fields

        private static readonly object _lock = new();

        private static bool _seeded = false;

        #endregion

        #region Protected Methods

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<CouponDbContext>));
                services.AddDbContext<CouponDbContext>((serviceProvider, options) =>
                {
                    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                    options.UseNpgsql(configuration.GetConnectionString("TestDbConnection"));
                });
                var serviceProvider = services.BuildServiceProvider();
                var scope = serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<CouponDbContext>();

                if (!_seeded)
                {
                    lock (_lock)
                    {
                        if (!_seeded)
                        {
                            dbContext.Database.EnsureDeleted();
                            Infrastructure.DependencyInjection.ApplyMigrations(serviceProvider);
                            _seeded = true; // Set the flag
                        }
                    }
                }
            });
        }

        #endregion
    }
}