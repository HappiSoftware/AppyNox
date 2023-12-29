using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppyNox.Services.Authentication.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Configures the entity type IdentityUser and seeds initial data.
    /// </summary>
    public class IdentityUserConfiguration : IEntityTypeConfiguration<IdentityUser>
    {
        private readonly string _adminUserId;

        /// <summary>
        /// Initializes a new instance of the IdentityUserConfiguration class with the specified admin user ID.
        /// </summary>
        /// <param name="adminUserId">The ID of the admin user for seeding data.</param>
        public IdentityUserConfiguration(string adminUserId)
        {
            _adminUserId = adminUserId;
        }

        /// <summary>
        /// Configures the IdentityUser entity type and seeds data.
        /// </summary>
        /// <param name="builder">The builder being used to construct the entity type model.</param>
        public void Configure(EntityTypeBuilder<IdentityUser> builder)
        {
            #region [ Configurations ]
            #endregion

            #region [ Seeds ]

            var hasher = new PasswordHasher<IdentityUser>();

            builder.HasData(new IdentityUser 
            { 
                Id = _adminUserId,
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@email.com",
                NormalizedEmail = "ADMIN@EMAIL.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(new IdentityUser(), "Admin@123")
            });

            #endregion
        }
    }
}
