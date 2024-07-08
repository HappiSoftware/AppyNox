using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppyNox.Services.Sso.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Configures the entity type IdentityRoleClaim and seeds initial data.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the IdentityRoleClaimConfiguration class with the specified admin role ID.
    /// </remarks>
    /// <param name="adminRoleId">The ID of the admin role for seeding data.</param>
    /// <param name="notAdminRoleId">The ID of the not admin role for seeding data.</param>
    internal class ApplicationRoleClaimConfiguration(Guid adminRoleId, Guid notAdminRoleId) : IEntityTypeConfiguration<IdentityRoleClaim<Guid>>
    {
        #region [ Fields ]

        private readonly Guid _adminRoleId = adminRoleId;
        private readonly Guid _notAdminRoleId = notAdminRoleId;

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
                new IdentityRoleClaim<Guid> { Id = 1, RoleId = _adminRoleId, ClaimType = "API.Permission", ClaimValue = "Users.View" },
                new IdentityRoleClaim<Guid> { Id = 2, RoleId = _adminRoleId, ClaimType = "API.Permission", ClaimValue = "Users.Create" },
                new IdentityRoleClaim<Guid> { Id = 3, RoleId = _adminRoleId, ClaimType = "API.Permission", ClaimValue = "Users.Edit" },
                new IdentityRoleClaim<Guid> { Id = 4, RoleId = _adminRoleId, ClaimType = "API.Permission", ClaimValue = "Users.Delete" },

                new IdentityRoleClaim<Guid> { Id = 5, RoleId = _adminRoleId, ClaimType = "API.Permission", ClaimValue = "Roles.View" },
                new IdentityRoleClaim<Guid> { Id = 6, RoleId = _adminRoleId, ClaimType = "API.Permission", ClaimValue = "Roles.Create" },
                new IdentityRoleClaim<Guid> { Id = 7, RoleId = _adminRoleId, ClaimType = "API.Permission", ClaimValue = "Roles.Edit" },
                new IdentityRoleClaim<Guid> { Id = 8, RoleId = _adminRoleId, ClaimType = "API.Permission", ClaimValue = "Roles.Delete" },
                new IdentityRoleClaim<Guid> { Id = 9, RoleId = _adminRoleId, ClaimType = "API.Permission", ClaimValue = "Roles.AssignPermission" },
                new IdentityRoleClaim<Guid> { Id = 10, RoleId = _adminRoleId, ClaimType = "API.Permission", ClaimValue = "Roles.WithdrawPermission" },

                new IdentityRoleClaim<Guid> { Id = 11, RoleId = _adminRoleId, ClaimType = "API.Permission", ClaimValue = "Coupons.View" },
                new IdentityRoleClaim<Guid> { Id = 12, RoleId = _adminRoleId, ClaimType = "API.Permission", ClaimValue = "Coupons.Create" },
                new IdentityRoleClaim<Guid> { Id = 13, RoleId = _adminRoleId, ClaimType = "API.Permission", ClaimValue = "Coupons.Edit" },
                new IdentityRoleClaim<Guid> { Id = 14, RoleId = _adminRoleId, ClaimType = "API.Permission", ClaimValue = "Coupons.Delete" },

                new IdentityRoleClaim<Guid> { Id = 15, RoleId = _adminRoleId, ClaimType = "API.Permission", ClaimValue = "Licenses.View" },
                new IdentityRoleClaim<Guid> { Id = 16, RoleId = _adminRoleId, ClaimType = "API.Permission", ClaimValue = "Licenses.Create" },
                new IdentityRoleClaim<Guid> { Id = 17, RoleId = _adminRoleId, ClaimType = "API.Permission", ClaimValue = "Licenses.Edit" },
                new IdentityRoleClaim<Guid> { Id = 18, RoleId = _adminRoleId, ClaimType = "API.Permission", ClaimValue = "Licenses.Delete" },

                new IdentityRoleClaim<Guid> { Id = 20, RoleId = _adminRoleId, ClaimType = "API.Permission", ClaimValue = "Products.View" },
                new IdentityRoleClaim<Guid> { Id = 21, RoleId = _adminRoleId, ClaimType = "API.Permission", ClaimValue = "Products.Create" },
                new IdentityRoleClaim<Guid> { Id = 22, RoleId = _adminRoleId, ClaimType = "API.Permission", ClaimValue = "Products.Edit" },
                new IdentityRoleClaim<Guid> { Id = 23, RoleId = _adminRoleId, ClaimType = "API.Permission", ClaimValue = "Products.Delete" },

                // NotAdmin Role Permissions
                new IdentityRoleClaim<Guid> { Id = 19, RoleId = _notAdminRoleId, ClaimType = "API.Permission", ClaimValue = "Coupons.View" }
            );

            #endregion
        }

        #endregion
    }
}