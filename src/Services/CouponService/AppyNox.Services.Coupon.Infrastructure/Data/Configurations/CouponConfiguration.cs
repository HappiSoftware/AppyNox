using AppyNox.Services.Coupon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppyNox.Services.Coupon.Infrastructure.Data.Configurations
{
    public class CouponConfiguration : IEntityTypeConfiguration<CouponEntity>
    {
        #region [ Public Methods ]

        public void Configure(EntityTypeBuilder<CouponEntity> builder)
        {
            builder.Property(x => x.MinAmount).IsRequired();
            builder.Property(x => x.Code).IsRequired().HasMaxLength(5);
            builder.Property(x => x.Description).HasMaxLength(60).IsUnicode().IsRequired();
            builder.Property(x => x.Detail).HasMaxLength(60).IsUnicode();
        }

        #endregion
    }
}