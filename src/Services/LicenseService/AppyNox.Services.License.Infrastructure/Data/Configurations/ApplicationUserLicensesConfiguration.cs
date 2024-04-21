using AppyNox.Services.License.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppyNox.Services.License.Infrastructure.Data.Configurations
{
    internal class ApplicationUserLicensesConfiguration : IEntityTypeConfiguration<ApplicationUserLicenses>
    {
        #region [ Public Methods ]

        public void Configure(EntityTypeBuilder<ApplicationUserLicenses> builder)
        {
            #region [ Configurations ]

            builder.HasKey(aul => aul.Id);

            builder.Property(c => c.Id)
                .ValueGeneratedOnAdd();

            builder.HasIndex(aul => new { aul.UserId, aul.LicenseId }).IsUnique();

            builder.Property(aul => aul.LicenseId).HasConversion(
                licenseId => licenseId.Value,
                value => new LicenseId(value));

            builder.HasOne(aul => aul.License)
                .WithMany(l => l.ApplicationUserLicenses)
                .HasForeignKey(aul => aul.LicenseId)
                .IsRequired();

            builder.HasMany(aul => aul.MacAddresses)
                .WithOne(aulm => aulm.ApplicationUserLicense)
                .HasForeignKey(aulm => aulm.ApplicationUserLicenseId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.LicenseId).IsRequired();

            #endregion
        }

        #endregion
    }
}