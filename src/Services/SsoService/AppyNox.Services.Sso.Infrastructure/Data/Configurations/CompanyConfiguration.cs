using AppyNox.Services.Sso.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppyNox.Services.Sso.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Configures the entity type Company and seeds initial data.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the Company class with the specified company ID.
    /// </remarks>
    /// <param name="happiCompanyId">The ID of the Happi Company for seeding data.</param>
    /// <param name="companyId">The ID of the Company for seeding data.</param>
    internal class CompanyConfiguration(Guid happiCompanyId, Guid companyId) : IEntityTypeConfiguration<Company>
    {
        #region [ Fields ]

        private readonly Guid _companyId = companyId;

        private readonly Guid _happiCompanyId = happiCompanyId;

        #endregion

        #region [ Public Methods ]

        /// <summary>
        /// Configures the Company entity type and seeds data.
        /// </summary>
        /// <param name="builder">The builder being used to construct the entity type model.</param>
        public void Configure(EntityTypeBuilder<Company> builder)
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

            builder.HasMany(c => c.EmailProviders)
                .WithOne()
                .HasForeignKey(c => c.CompanyId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Name).IsRequired();

            #endregion

            #region [ Seeds ]

            builder.HasData(
                new Company
                {
                    Id = _happiCompanyId,
                    Name = "HappiSoft"
                },
                new Company
                {
                    Id = _companyId,
                    Name = "TestCompany"
                });

            #endregion
        }

        #endregion
    }
}