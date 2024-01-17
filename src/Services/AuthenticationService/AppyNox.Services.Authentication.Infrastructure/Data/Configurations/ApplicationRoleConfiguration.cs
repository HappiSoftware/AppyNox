using AppyNox.Services.Authentication.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppyNox.Services.Authentication.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Configures the entity type ApplicationRoleConfiguration and seeds initial data.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the ApplicationRoleConfiguration class with the specified admin role ID.
    /// </remarks>
    /// <param name="adminRoleId">The ID of the admin role for seeding data.</param>
    /// <param name="companyId">The ID of the admin role company for seeding data.</param>
    /// <param name="superAdminRoleId">The ID of the super admin role for seeding data.</param>
    /// <param name="happiCompanyId">The ID of the super admin role company for seeding data.</param>
    internal class ApplicationRoleConfiguration(Guid adminRoleId, Guid companyId, Guid superAdminRoleId, Guid happiCompanyId)
        : IEntityTypeConfiguration<ApplicationRole>
    {
        #region [ Fields ]

        private readonly Guid _adminRoleId = adminRoleId;

        private readonly Guid _companyId = companyId;

        private readonly Guid _superAdminRoleId = superAdminRoleId;

        private readonly Guid _happiCompanyId = happiCompanyId;

        #endregion

        #region [ Public Methods ]

        /// <summary>
        /// Configures the ApplicationRole entity type and seeds data.
        /// </summary>
        /// <param name="builder">The builder being used to construct the entity type model.</param>
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            #region [ Configurations ]

            builder.Property(x => x.Code).IsRequired().HasMaxLength(5);
            builder.Property(x => x.Description).HasMaxLength(60);

            builder.HasOne(ar => ar.Company)
                .WithMany(c => c.Roles)
                .HasForeignKey(ar => ar.CompanyId)
                .IsRequired();

            #endregion

            #region [ Seeds ]

            builder.HasData(new ApplicationRole
            {
                Id = _adminRoleId,
                Code = "Role1",
                Name = "Admin",
                Description = "RoleDescription",
                NormalizedName = "ADMIN",
                CompanyId = _companyId
            },
            new ApplicationRole
            {
                Id = _superAdminRoleId,
                Code = "Role2",
                Name = "SuperAdmin",
                Description = "RoleDescription",
                NormalizedName = "SUPERADMIN",
                CompanyId = _happiCompanyId
            });

            #endregion
        }

        #endregion
    }
}