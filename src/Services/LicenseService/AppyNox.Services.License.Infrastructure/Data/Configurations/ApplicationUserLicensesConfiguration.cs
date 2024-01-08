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

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .ValueGeneratedOnAdd();

            builder.HasOne(aul => aul.License)
                .WithMany(l => l.ApplicationUserLicenses)
                .HasForeignKey(aul => aul.LicenseId)
                .IsRequired();

            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.MacAddress).IsRequired();
            builder.Property(x => x.LicenseId).IsRequired();

            #endregion
        }

        #endregion
    }
}