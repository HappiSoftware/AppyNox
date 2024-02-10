using AppyNox.Services.Sso.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppyNox.Services.Sso.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Configures the entity type ApplicationUser and seeds initial data.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the ApplicationUserConfiguration class with the specified admin user ID.
    /// </remarks>
    /// <param name="adminUserId">The ID of the admin user for seeding data.</param>
    /// <param name="companyId">The ID of the admin user company for seeding data.</param>
    /// <param name="superAdminId">The ID of the super admin user for seeding data.</param>
    /// <param name="happiCompanyId">The ID of the super admin company user for seeding data.</param>
    internal class ApplicationUserConfiguration(Guid adminUserId, Guid companyId, Guid superAdminId, Guid happiCompanyId)
        : IEntityTypeConfiguration<ApplicationUser>
    {
        #region [ Fields ]

        private readonly Guid _adminUserId = adminUserId;

        private readonly Guid _superAdminId = superAdminId;

        private readonly Guid _companyId = companyId;

        private readonly Guid _happiCompanyId = happiCompanyId;

        #endregion

        #region [ Public Methods ]

        /// <summary>
        /// Configures the ApplicationUser entity type and seeds data.
        /// </summary>
        /// <param name="builder">The builder being used to construct the entity type model.</param>
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            #region [ Configurations ]

            builder.Property(x => x.Code).IsRequired().HasMaxLength(5);

            builder.HasOne(c => c.Company)
                .WithMany(cd => cd.Users)
                .HasForeignKey(c => c.CompanyId)
                .IsRequired();

            #endregion

            #region [ Seeds ]

            var hasher = new PasswordHasher<ApplicationUser>();

            builder.HasData(new ApplicationUser
            {
                Id = _adminUserId,
                Code = "USR01",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@email.com",
                NormalizedEmail = "ADMIN@EMAIL.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(new ApplicationUser(), "Admin@123"),
                IsAdmin = true,
                CompanyId = _companyId,
                SecurityStamp = Guid.NewGuid().ToString()
            },
            new ApplicationUser
            {
                Id = _superAdminId,
                Code = "USR02",
                UserName = "superadmin",
                NormalizedUserName = "SUPERADMIN",
                Email = "sadmin@email.com",
                NormalizedEmail = "SADMIN@EMAIL.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(new ApplicationUser(), "SAdmin@123"),
                IsAdmin = true,
                CompanyId = _happiCompanyId,
                SecurityStamp = Guid.NewGuid().ToString()
            });

            #endregion
        }

        #endregion
    }
}