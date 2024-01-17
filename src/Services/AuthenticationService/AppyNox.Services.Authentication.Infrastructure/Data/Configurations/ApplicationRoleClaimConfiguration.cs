using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppyNox.Services.Authentication.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Configures the entity type IdentityRoleClaim and seeds initial data.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the IdentityRoleClaimConfiguration class with the specified admin role ID.
    /// </remarks>
    /// <param name="adminRoleId">The ID of the admin role for seeding data.</param>
    internal class ApplicationRoleClaimConfiguration(Guid adminRoleId) : IEntityTypeConfiguration<IdentityRoleClaim<Guid>>
    {
        #region [ Fields ]

        private readonly Guid _adminRoleId = adminRoleId;

        #endregion

        #region [ Public Methods ]

        /// <summary>
        /// Configures the IdentityRoleClaim entity type and seeds data.
        /// </summary>
        /// <param name="builder">The builder being used to construct the entity type model.</param>
        public void Configure(EntityTypeBuilder<IdentityRoleClaim<Guid>> builder)
        {
            #region [ Seeds ]

            builder.HasData(
                new IdentityRoleClaim<Guid> { Id = 1, RoleId = _adminRoleId, ClaimType = "Permission", ClaimValue = "Users.View" },
                new IdentityRoleClaim<Guid> { Id = 2, RoleId = _adminRoleId, ClaimType = "Permission", ClaimValue = "Users.Create" },
                new IdentityRoleClaim<Guid> { Id = 3, RoleId = _adminRoleId, ClaimType = "Permission", ClaimValue = "Users.Edit" },
                new IdentityRoleClaim<Guid> { Id = 4, RoleId = _adminRoleId, ClaimType = "Permission", ClaimValue = "Users.Delete" },

                new IdentityRoleClaim<Guid> { Id = 5, RoleId = _adminRoleId, ClaimType = "Permission", ClaimValue = "Roles.View" },
                new IdentityRoleClaim<Guid> { Id = 6, RoleId = _adminRoleId, ClaimType = "Permission", ClaimValue = "Roles.Create" },
                new IdentityRoleClaim<Guid> { Id = 7, RoleId = _adminRoleId, ClaimType = "Permission", ClaimValue = "Roles.Edit" },
                new IdentityRoleClaim<Guid> { Id = 8, RoleId = _adminRoleId, ClaimType = "Permission", ClaimValue = "Roles.Delete" },
                new IdentityRoleClaim<Guid> { Id = 9, RoleId = _adminRoleId, ClaimType = "Permission", ClaimValue = "Roles.AssignPermission" },
                new IdentityRoleClaim<Guid> { Id = 10, RoleId = _adminRoleId, ClaimType = "Permission", ClaimValue = "Roles.WithdrawPermission" },

                new IdentityRoleClaim<Guid> { Id = 11, RoleId = _adminRoleId, ClaimType = "Permission", ClaimValue = "Coupons.View" },
                new IdentityRoleClaim<Guid> { Id = 12, RoleId = _adminRoleId, ClaimType = "Permission", ClaimValue = "Coupons.Create" },
                new IdentityRoleClaim<Guid> { Id = 13, RoleId = _adminRoleId, ClaimType = "Permission", ClaimValue = "Coupons.Edit" },
                new IdentityRoleClaim<Guid> { Id = 14, RoleId = _adminRoleId, ClaimType = "Permission", ClaimValue = "Coupons.Delete" },

                new IdentityRoleClaim<Guid> { Id = 15, RoleId = _adminRoleId, ClaimType = "Permission", ClaimValue = "Licenses.View" },
                new IdentityRoleClaim<Guid> { Id = 16, RoleId = _adminRoleId, ClaimType = "Permission", ClaimValue = "Licenses.Create" },
                new IdentityRoleClaim<Guid> { Id = 17, RoleId = _adminRoleId, ClaimType = "Permission", ClaimValue = "Licenses.Edit" },
                new IdentityRoleClaim<Guid> { Id = 18, RoleId = _adminRoleId, ClaimType = "Permission", ClaimValue = "Licenses.Delete" }
            );

            #endregion
        }

        #endregion
    }
}