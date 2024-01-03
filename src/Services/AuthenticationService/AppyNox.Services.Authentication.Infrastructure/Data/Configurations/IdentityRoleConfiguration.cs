using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppyNox.Services.Authentication.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Configures the entity type IdentityRole and seeds initial data.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the IdentityRoleConfiguration class with the specified admin role ID.
    /// </remarks>
    /// <param name="adminRoleId">The ID of the admin role for seeding data.</param>
    internal class IdentityRoleConfiguration(string adminRoleId) : IEntityTypeConfiguration<IdentityRole>
    {
        #region [ Fields ]

        private readonly string _adminRoleId = adminRoleId;

        #endregion

        #region [ Public Methods ]

        /// <summary>
        /// Configures the IdentityRole entity type and seeds data.
        /// </summary>
        /// <param name="builder">The builder being used to construct the entity type model.</param>
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            #region [ Seeds ]

            builder.HasData(new IdentityRole
            {
                Id = _adminRoleId,
                Name = "Admin",
                NormalizedName = "ADMIN"
            });

            #endregion
        }

        #endregion
    }
}