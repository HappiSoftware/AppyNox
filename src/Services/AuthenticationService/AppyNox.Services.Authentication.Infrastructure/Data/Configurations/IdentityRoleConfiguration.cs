using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppyNox.Services.Authentication.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Configures the entity type IdentityRole and seeds initial data.
    /// </summary>
    public class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        private readonly string _adminRoleId;

        /// <summary>
        /// Initializes a new instance of the IdentityRoleConfiguration class with the specified admin role ID.
        /// </summary>
        /// <param name="adminRoleId">The ID of the admin role for seeding data.</param>
        public IdentityRoleConfiguration(string adminRoleId)
        {
            _adminRoleId = adminRoleId;
        }

        /// <summary>
        /// Configures the IdentityRole entity type and seeds data.
        /// </summary>
        /// <param name="builder">The builder being used to construct the entity type model.</param>
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            #region [ Configurations ]
            #endregion

            #region [ Seeds ]

            builder.HasData(new IdentityRole 
            { 
                Id = _adminRoleId, 
                Name = "Admin", 
                NormalizedName = "ADMIN" 
            });

            #endregion
        }
    }
}
