using AppyNox.Services.Authentication.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppyNox.Services.Authentication.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Configures the entity type ApplicationUser and seeds initial data.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the ApplicationUserConfiguration class with the specified admin user ID.
    /// </remarks>
    /// <param name="adminUserId">The ID of the admin user for seeding data.</param>
    internal class ApplicationUserConfiguration(string adminUserId, Guid companyId) : IEntityTypeConfiguration<ApplicationUser>
    {
        #region [ Fields ]

        private readonly string _adminUserId = adminUserId;

        private readonly Guid _companyId = companyId;

        #endregion

        #region [ Public Methods ]

        /// <summary>
        /// Configures the ApplicationUser entity type and seeds data.
        /// </summary>
        /// <param name="builder">The builder being used to construct the entity type model.</param>
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasOne(c => c.Company)
                .WithMany(cd => cd.Users)
                .HasForeignKey(c => c.CompanyId)
                .IsRequired();

            #region [ Seeds ]

            var hasher = new PasswordHasher<ApplicationUser>();

            builder.HasData(new ApplicationUser
            {
                Id = _adminUserId,
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@email.com",
                NormalizedEmail = "ADMIN@EMAIL.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(new ApplicationUser(), "Admin@123"),
                CompanyId = _companyId
            });

            #endregion
        }

        #endregion
    }
}