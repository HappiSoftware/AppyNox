using AppyNox.Services.Authentication.Domain.Entities;
using AppyNox.Services.Authentication.Infrastructure.Data.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppyNox.Services.Authentication.Infrastructure.Data
{
    /// <summary>
    /// Represents the database context for identity-related entities, extending the IdentityDbContext with custom configurations.
    /// </summary>
    public class IdentityDatabaseContext
    : IdentityDbContext<
                        ApplicationUser,            // User entity
                        ApplicationRole,            // Role entity
                        Guid,                       // Key type
                        IdentityUserClaim<Guid>,    // User claims
                        IdentityUserRole<Guid>,     // User-Role join entity
                        IdentityUserLogin<Guid>,    // User logins
                        IdentityRoleClaim<Guid>,    // Role claims
                        IdentityUserToken<Guid>     // User tokens
                        >
    {
        #region [ Public Constructors ]

        public IdentityDatabaseContext()
        {
        }

        public IdentityDatabaseContext(DbContextOptions<IdentityDatabaseContext> options) : base(options)
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

            Guid adminUserId = Guid.Parse("a8bfc75b-2ac3-47e2-b013-8b8a1efba45d");
            Guid superAdminUserId = Guid.Parse("6e54d3e3-90d0-4604-91b4-77009cedd760");

            Guid adminRoleId = Guid.Parse("e24e99e7-00e4-4007-a042-565eac12d96d");
            Guid superAdminRoleId = Guid.Parse("f51a5d58-ff38-4563-9d32-f658ef2b40d0");

            Guid companyId = Guid.Parse("221e8b2c-59d5-4e5b-b010-86c239b66738");
            Guid happiCompanyId = Guid.Parse("0ebae1bf-6610-4967-a8ed-b149219caf68");

            #endregion

            #region [ Entity Configurations ]

            builder.ApplyConfiguration(new CompanyConfiguration(happiCompanyId, companyId));
            builder.ApplyConfiguration(new ApplicationRoleConfiguration(adminRoleId, companyId, superAdminRoleId, happiCompanyId));
            builder.ApplyConfiguration(new ApplicationUserConfiguration(adminUserId, companyId, superAdminUserId, happiCompanyId));
            builder.ApplyConfiguration(new ApplicationRoleClaimConfiguration(adminRoleId));
            builder.ApplyConfiguration(new ApplicationUserRoleConfiguration(adminRoleId, adminUserId, superAdminRoleId, superAdminUserId));

            #endregion
        }

        #endregion
    }
}