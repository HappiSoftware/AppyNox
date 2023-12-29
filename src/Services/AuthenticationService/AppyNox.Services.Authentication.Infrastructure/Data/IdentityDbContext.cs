using AppyNox.Services.Authentication.Infrastructure.Data.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AppyNox.Services.Authentication.Infrastructure.Data
{
    /// <summary>
    /// Represents the database context for identity-related entities, extending the IdentityDbContext with custom configurations.
    /// </summary>
    public class IdentityDbContext : IdentityDbContext<IdentityUser>
    {
        #region [ Public Constructors ]

        public IdentityDbContext()
        {
        }

        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {
        }

        #endregion

        #region [ Protected Methods ]

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Get environment
                string? environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                Console.WriteLine($"Environment: {environment}");

                // Build config
                IConfiguration config = new ConfigurationBuilder()
                    .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../AppyNox.Services.Authentication.WebAPI"))
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{environment}.json", optional: true)
                    .AddEnvironmentVariables()
                    .Build();
                var connectionString = config.GetConnectionString("Default");
                Console.WriteLine($"Connection String: {connectionString}");
                optionsBuilder.UseNpgsql(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region [ Common Ids ]

            string adminUserId = Guid.NewGuid().ToString();
            string adminRoleId = Guid.NewGuid().ToString();

            #endregion

            #region [ Entity Configurations ]

            builder.ApplyConfiguration(new IdentityRoleConfiguration(adminRoleId));
            builder.ApplyConfiguration(new IdentityUserConfiguration(adminUserId));
            builder.ApplyConfiguration(new IdentityRoleClaimConfiguration(adminRoleId));
            builder.ApplyConfiguration(new IdentityUserRoleConfiguration(adminRoleId, adminUserId));

            #endregion
        }

        #endregion
    }
}