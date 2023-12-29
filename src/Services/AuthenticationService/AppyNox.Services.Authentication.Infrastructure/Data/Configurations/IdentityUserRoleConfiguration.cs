using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppyNox.Services.Authentication.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Configures the entity type IdentityUserRole and seeds initial data.
    /// </summary>
    public class IdentityUserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        private readonly string _adminRoleId;
        private readonly string _adminUserId;

        /// <summary>
        /// Initializes a new instance of the IdentityUserRoleConfiguration class with the specified admin role and user IDs.
        /// </summary>
        /// <param name="adminRoleId">The ID of the admin role for seeding data.</param>
        /// <param name="adminUserId">The ID of the admin user for seeding data.</param>
        public IdentityUserRoleConfiguration(string adminRoleId, string adminUserId)
        {
            _adminRoleId = adminRoleId;
            _adminUserId = adminUserId;

        }

        /// <summary>
        /// Configures the IdentityUserRole entity type and seeds data.
        /// </summary>
        /// <param name="builder">The builder being used to construct the entity type model.</param>
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            #region [ Configurations ]
            #endregion

            #region [ Seeds ]

            builder.HasData(
                new IdentityUserRole<string> { RoleId = _adminRoleId, UserId = _adminUserId }
            );

            #endregion
        }
    }
}
