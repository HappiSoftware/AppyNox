using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppyNox.Services.Sso.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Configures the entity type IdentityUserRole and seeds initial data.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the IdentityUserRoleConfiguration class with the specified admin role and user IDs.
    /// </remarks>
    /// <param name="adminRoleId">The ID of the admin role for seeding data.</param>
    /// <param name="adminUserId">The ID of the admin user for seeding data.</param>
    /// <param name="superAdminRoleId">The ID of the super admin role for seeding data.</param>
    /// <param name="superAdminUserId">The ID of the super admin user for seeding data.</param>
    /// <param name="notAdminRoleId">The ID of the not admin user for seeding data.</param>
    /// <param name="notAdminUserId">The ID of the not admin user for seeding data.</param>
    internal class ApplicationUserRoleConfiguration(Guid adminRoleId, Guid adminUserId, Guid superAdminRoleId, Guid superAdminUserId, Guid notAdminRoleId, Guid notAdminUserId)
        : IEntityTypeConfiguration<IdentityUserRole<Guid>>
    {
        #region [ Fields ]

        private readonly Guid _adminRoleId = adminRoleId;

        private readonly Guid _adminUserId = adminUserId;

        private readonly Guid _superAdminRoleId = superAdminRoleId;

        private readonly Guid _superAdminUserId = superAdminUserId;

        private readonly Guid _notAdminRoleId = notAdminRoleId;

        private readonly Guid _notAdminUserId = notAdminUserId;

        #endregion

        #region [ Public Methods ]

        /// <summary>
        /// Configures the IdentityUserRole entity type and seeds data.
        /// </summary>
        /// <param name="builder">The builder being used to construct the entity type model.</param>
        public void Configure(EntityTypeBuilder<IdentityUserRole<Guid>> builder)
        {
            #region [ Seeds ]

            builder.HasData(
                new IdentityUserRole<Guid> { RoleId = _adminRoleId, UserId = _adminUserId },
                new IdentityUserRole<Guid> { RoleId = _superAdminRoleId, UserId = _superAdminUserId },
                new IdentityUserRole<Guid> { RoleId = _notAdminRoleId, UserId = _notAdminUserId }
            );

            #endregion
        }

        #endregion
    }
}