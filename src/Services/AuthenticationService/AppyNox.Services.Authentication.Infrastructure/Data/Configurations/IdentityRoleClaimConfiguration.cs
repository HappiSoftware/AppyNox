using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppyNox.Services.Authentication.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Configures the entity type IdentityRoleClaim and seeds initial data.
    /// </summary>
    public class IdentityRoleClaimConfiguration : IEntityTypeConfiguration<IdentityRoleClaim<string>>
    {
        private readonly string _adminRoleId;

        /// <summary>
        /// Initializes a new instance of the IdentityRoleClaimConfiguration class with the specified admin role ID.
        /// </summary>
        /// <param name="adminRoleId">The ID of the admin role for seeding data.</param>
        public IdentityRoleClaimConfiguration(string adminRoleId)
        {
            _adminRoleId = adminRoleId;
        }

        /// <summary>
        /// Configures the IdentityRoleClaim entity type and seeds data.
        /// </summary>
        /// <param name="builder">The builder being used to construct the entity type model.</param>
        public void Configure(EntityTypeBuilder<IdentityRoleClaim<string>> builder)
        {
            #region [ Configurations ]
            #endregion

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
                new IdentityRoleClaim<string> { Id = 14, RoleId = _adminRoleId, ClaimType = "Permission", ClaimValue = "Coupons.Delete" }
            );

            #endregion
        }
    }
}
