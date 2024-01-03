using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppyNox.Services.Authentication.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Configures the entity type IdentityUserRole and seeds initial data.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the IdentityUserRoleConfiguration class with the specified admin role and user IDs.
    /// </remarks>
    /// <param name="adminRoleId">The ID of the admin role for seeding data.</param>
    /// <param name="adminUserId">The ID of the admin user for seeding data.</param>
    internal class IdentityUserRoleConfiguration(string adminRoleId, string adminUserId) : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        #region [ Fields ]

        private readonly string _adminRoleId = adminRoleId;

        private readonly string _adminUserId = adminUserId;

        #endregion

        #region [ Public Methods ]

        /// <summary>
        /// Configures the IdentityUserRole entity type and seeds data.
        /// </summary>
        /// <param name="builder">The builder being used to construct the entity type model.</param>
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            #region [ Seeds ]

            builder.HasData(
                new IdentityUserRole<string> { RoleId = _adminRoleId, UserId = _adminUserId }
            );

            #endregion
        }

        #endregion
    }
}