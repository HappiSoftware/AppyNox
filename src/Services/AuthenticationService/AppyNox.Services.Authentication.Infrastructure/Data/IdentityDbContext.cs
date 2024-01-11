using AppyNox.Services.Authentication.Domain.Entities;
using AppyNox.Services.Authentication.Infrastructure.Data.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppyNox.Services.Authentication.Infrastructure.Data
{
    /// <summary>
    /// Represents the database context for identity-related entities, extending the IdentityDbContext with custom configurations.
    /// </summary>
    public class IdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        #region [ Public Constructors ]

        public IdentityDbContext()
        {
        }

        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {
        }

        #endregion

        #region [ Properties ]

        public DbSet<CompanyEntity> Companies { get; set; }

        #endregion

        #region [ Protected Methods ]

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region [ Common Ids ]

            string adminUserId = "a8bfc75b-2ac3-47e2-b013-8b8a1efba45d";
            string adminRoleId = "e24e99e7-00e4-4007-a042-565eac12d96d";
            Guid companyId = Guid.Parse("221e8b2c-59d5-4e5b-b010-86c239b66738");

            #endregion

            #region [ Entity Configurations ]

            builder.ApplyConfiguration(new CompanyConfiguration(companyId));
            builder.ApplyConfiguration(new IdentityRoleConfiguration(adminRoleId));
            builder.ApplyConfiguration(new ApplicationUserConfiguration(adminUserId, companyId));
            builder.ApplyConfiguration(new IdentityRoleClaimConfiguration(adminRoleId));
            builder.ApplyConfiguration(new IdentityUserRoleConfiguration(adminRoleId, adminUserId));

            #endregion
        }

        #endregion
    }
}