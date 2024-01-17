using AppyNox.Services.Authentication.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppyNox.Services.Authentication.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Configures the entity type CompanyEntity and seeds initial data.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the CompanyEntity class with the specified company ID.
    /// </remarks>
    /// <param name="happiCompanyId">The ID of the Happi CompanyEntity for seeding data.</param>
    /// <param name="companyId">The ID of the CompanyEntity for seeding data.</param>
    internal class CompanyConfiguration(Guid happiCompanyId, Guid companyId) : IEntityTypeConfiguration<CompanyEntity>
    {
        #region [ Fields ]

        private readonly Guid _companyId = companyId;

        private readonly Guid _happiCompanyId = happiCompanyId;

        #endregion

        #region [ Public Methods ]

        /// <summary>
        /// Configures the CompanyEntity entity type and seeds data.
        /// </summary>
        /// <param name="builder">The builder being used to construct the entity type model.</param>
        public void Configure(EntityTypeBuilder<CompanyEntity> builder)
        {
            #region [ Configurations ]

            builder.HasKey(c => c.Id);

            builder.Property(x => x.Code).IsRequired().HasMaxLength(5);

            builder.Property(c => c.Id)
                .ValueGeneratedOnAdd();

            builder.HasMany(c => c.Users)
                .WithOne(cd => cd.Company)
                .HasForeignKey(c => c.CompanyId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Roles)
                .WithOne(cd => cd.Company)
                .HasForeignKey(c => c.CompanyId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Name).IsRequired();

            #endregion

            #region [ Seeds ]

            builder.HasData(
                new CompanyEntity
                {
                    Id = _happiCompanyId,
                    Name = "HappiSoft"
                },
                new CompanyEntity
                {
                    Id = _companyId,
                    Name = "TestCompany"
                });

            #endregion
        }

        #endregion
    }
}