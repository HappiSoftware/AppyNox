using AppyNox.Services.License.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppyNox.Services.License.Infrastructure.Data.Configurations
{
    internal class LicenseConfiguration(Guid licenseId) : IEntityTypeConfiguration<LicenseEntity>
    {
        #region [ Fields ]

        private readonly Guid _licenseId = licenseId;

        #endregion

        #region [ Public Methods ]

        public void Configure(EntityTypeBuilder<LicenseEntity> builder)
        {
            #region [ Configurations ]

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .ValueGeneratedOnAdd();

            builder.HasMany(l => l.ApplicationUserLicenses)
                .WithOne(aul => aul.License)
                .HasForeignKey(aul => aul.LicenseId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Code).IsRequired().HasMaxLength(5);
            builder.Property(x => x.Description).HasMaxLength(60).IsUnicode().IsRequired();
            builder.Property(x => x.LicenseKey).IsRequired();
            builder.Property(x => x.ExpirationDate).IsRequired();
            builder.Property(x => x.MaxUsers).IsRequired();
            builder.Property(x => x.MaxMacAddresses).IsRequired();

            #endregion

            #region [ Seeds ]

            builder.HasData(
                new LicenseEntity
                {
                    Id = _licenseId,
                    Code = "LK001",
                    Description = "License Description",
                    LicenseKey = "7f033381-fbf7-4929-b5f7-c64261b20bf3",
                    CompanyId = Guid.Parse("221e8b2c-59d5-4e5b-b010-86c239b66738"),
                    ExpirationDate = DateTime.UtcNow.AddDays(365),
                    MaxUsers = 3,
                    MaxMacAddresses = 1
                });

            #endregion
        }

        #endregion
    }
}