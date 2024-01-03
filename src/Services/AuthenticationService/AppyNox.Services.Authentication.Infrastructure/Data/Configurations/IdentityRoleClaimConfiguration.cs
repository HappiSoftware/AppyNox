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
    internal class IdentityRoleClaimConfiguration(string adminRoleId) : IEntityTypeConfiguration<IdentityRoleClaim<string>>
    {
        #region [ Fields ]

        private readonly string _adminRoleId = adminRoleId;

        #endregion

        #region [ Public Methods ]

        /// <summary>
        /// Configures the IdentityRoleClaim entity type and seeds data.
        /// </summary>
        /// <param name="builder">The builder being used to construct the entity type model.</param>
        public void Configure(EntityTypeBuilder<IdentityRoleClaim<string>> builder)
        {
            #region [ Seeds ]

            builder.HasData(
                new IdentityRoleClaim<string> { Id = 1, RoleId = _adminRoleId, ClaimType = "Permission", ClaimValue = "Users.View" },
                new IdentityRoleClaim<string> { Id = 2, RoleId = _adminRoleId, ClaimType = "Permission", ClaimValue = "Users.Create" },
                new IdentityRoleClaim<string> { Id = 3, RoleId = _adminRoleId, ClaimType = "Permission", ClaimValue = "Users.Edit" },
                new IdentityRoleClaim<string> { Id = 4, RoleId = _adminRoleId, ClaimType = "Permission", ClaimValue = "Users.Delete" },

                new IdentityRoleClaim<string> { Id = 5, RoleId = _adminRoleId, ClaimType = "Permission", ClaimValue = "Roles.View" },
                new IdentityRoleClaim<string> { Id = 6, RoleId = _adminRoleId, ClaimType = "Permission", ClaimValue = "Roles.Create" },
                new IdentityRoleClaim<string> { Id = 7, RoleId = _adminRoleId, ClaimType = "Permission", ClaimValue = "Roles.Edit" },
                new IdentityRoleClaim<string> { Id = 8, RoleId = _adminRoleId, ClaimType = "Permission", ClaimValue = "Roles.Delete" },
                new IdentityRoleClaim<string> { Id = 9, RoleId = _adminRoleId, ClaimType = "Permission", ClaimValue = "Roles.AssignPermission" },
                new IdentityRoleClaim<string> { Id = 10, RoleId = _adminRoleId, ClaimType = "Permission", ClaimValue = "Roles.WithdrawPermission" },

                new IdentityRoleClaim<string> { Id = 11, RoleId = _adminRoleId, ClaimType = "Permission", ClaimValue = "Coupons.View" },
                new IdentityRoleClaim<string> { Id = 12, RoleId = _adminRoleId, ClaimType = "Permission", ClaimValue = "Coupons.Create" },
                new IdentityRoleClaim<string> { Id = 13, RoleId = _adminRoleId, ClaimType = "Permission", ClaimValue = "Coupons.Edit" },
                new IdentityRoleClaim<string> { Id = 14, RoleId = _adminRoleId, ClaimType = "Permission", ClaimValue = "Coupons.Delete" },

                new IdentityRoleClaim<string> { Id = 15, RoleId = _adminRoleId, ClaimType = "Permission", ClaimValue = "Licenses.View" },
                new IdentityRoleClaim<string> { Id = 16, RoleId = _adminRoleId, ClaimType = "Permission", ClaimValue = "Licenses.Create" },
                new IdentityRoleClaim<string> { Id = 17, RoleId = _adminRoleId, ClaimType = "Permission", ClaimValue = "Licenses.Edit" },
                new IdentityRoleClaim<string> { Id = 18, RoleId = _adminRoleId, ClaimType = "Permission", ClaimValue = "Licenses.Delete" }
            );

            #endregion
        }

        #endregion
    }
}