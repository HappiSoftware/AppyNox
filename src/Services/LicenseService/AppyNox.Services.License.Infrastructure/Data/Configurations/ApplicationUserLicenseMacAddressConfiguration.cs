using AppyNox.Services.License.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppyNox.Services.License.Infrastructure.Data.Configurations
{
    internal class ApplicationUserLicenseMacAddressConfiguration : IEntityTypeConfiguration<ApplicationUserLicenseMacAddress>
    {
        #region [ Public Methods ]

        public void Configure(EntityTypeBuilder<ApplicationUserLicenseMacAddress> builder)
        {
            #region [ Configurations ]

            builder.HasKey(c => c.Id);

            builder.Property(x => x.MacAddress).IsRequired();

            builder.HasOne(x => x.ApplicationUserLicense)
                .WithMany(aul => aul.MacAddresses)
                .HasForeignKey(x => x.ApplicationUserLicenseId)
                .IsRequired();

            #endregion
        }

        #endregion
    }
}