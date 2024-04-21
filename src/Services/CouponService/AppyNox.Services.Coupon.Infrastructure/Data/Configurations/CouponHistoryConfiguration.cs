using AppyNox.Services.Coupon.Domain.Coupons;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppyNox.Services.Coupon.Infrastructure.Data.Configurations;

internal sealed class CouponHistoryConfiguration(CouponId couponId) : IEntityTypeConfiguration<CouponHistory>
{
    #region [ Public Methods ]

    public void Configure(EntityTypeBuilder<CouponHistory> builder)
    {
        #region [ Configuration ]

        builder.HasKey(a => a.Id);

        builder.Property(o => o.Id).HasConversion(
            _offerOptionalServiceId => _offerOptionalServiceId.Value,
            value => new CouponHistoryId(value));

        builder.Property(osh => osh.CouponId).IsRequired();
        builder.Property(osh => osh.MinimumAmount).IsRequired();
        builder.Property(osh => osh.Date).IsRequired();

        #endregion

        #region [ SeedData ]

        builder.HasData(new
        {
            Id = new CouponHistoryId(Guid.NewGuid()),
            Date = DateTime.UtcNow,
            CouponId = couponId,
            MinimumAmount = 100,
            CreatedBy = "System",
            CreationDate = DateTime.SpecifyKind(new DateTime(2024, 4, 21), DateTimeKind.Utc),
            UpdatedBy = (string?)null,
            UpdateDate = (DateTime?)null
        });

        #endregion
    }

    #endregion
}