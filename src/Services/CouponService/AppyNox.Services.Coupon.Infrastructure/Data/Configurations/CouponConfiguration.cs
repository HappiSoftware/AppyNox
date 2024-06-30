using AppyNox.Services.Coupon.Domain.Coupons;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppyNox.Services.Coupon.Infrastructure.Data.Configurations;

internal class CouponConfiguration(CouponId couponId1, CouponId couponId2, CouponDetailId couponDetailId) : IEntityTypeConfiguration<Domain.Coupons.Coupon>
{
    #region [ Public Methods ]

    public void Configure(EntityTypeBuilder<Domain.Coupons.Coupon> builder)
    {
        #region [ Configurations ]

        builder.HasKey(c => c.Id);

        builder.Property(o => o.Id).HasConversion(
            couponId => couponId.Value,
            value => new CouponId(value));

        builder.Property(o => o.CouponDetailId).HasConversion(
                couponDetailId => couponDetailId.Value,
                value => new CouponDetailId(value));

        builder.HasOne(c => c.CouponDetail)
            .WithMany()
            .HasForeignKey(c => c.CouponDetailId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(b => b.CouponDetail).IsRequired();

        builder.Property(x => x.Code).IsRequired().HasMaxLength(5);
        builder.Property(x => x.Description).HasMaxLength(60).IsUnicode().IsRequired();
        builder.Property(x => x.Detail).HasMaxLength(60).IsUnicode();

        builder.OwnsOne(o => o.Amount, amountBuilder =>
        {
            amountBuilder.WithOwner();
            amountBuilder.Property(c => c.MinAmount).IsRequired();
            amountBuilder.Property(c => c.DiscountAmount);
        });

        builder.HasMany(c => c.Histories)
            .WithOne()
            .HasForeignKey(ch => ch.CouponId)
            .OnDelete(DeleteBehavior.Cascade);

        #endregion

        #region [ Seeds ]

        builder.HasData(
            new
            {
                Id = couponId1,
                Code = "EXF50",
                Description = "Description",
                Detail = "Detail1",
                CouponDetailId = couponDetailId,
                CreatedBy = "System",
                CreationDate = DateTime.SpecifyKind(new DateTime(2024, 4, 21), DateTimeKind.Utc),
                UpdatedBy = (string?)null,
                UpdateDate = (DateTime?)null,
                IsDeleted = false,
                DeletedDate = (DateTime?)null,
                DeletedBy = (string?)null
            });
        builder.OwnsOne(o => o.Amount).HasData(new
        {
            CouponId = couponId1,
            DiscountAmount = 10.65,
            MinAmount = 100
        });

        // Second Coupon

        builder.HasData(
            new
            {
                Id = couponId2,
                Code = "EXF60",
                Description = "Description2",
                DiscountAmount = 20.55,
                MinAmount = 200,
                Detail = "Detail2",
                CouponDetailId = couponDetailId,
                CreatedBy = "System",
                CreationDate = DateTime.SpecifyKind(new DateTime(2024, 4, 21), DateTimeKind.Utc),
                UpdatedBy = (string?)null,
                UpdateDate = (DateTime?)null,
                IsDeleted = false,
                DeletedDate = (DateTime?)null,
                DeletedBy = (string?)null
            });
        builder.OwnsOne(o => o.Amount).HasData(new
        {
            CouponId = couponId2,
            DiscountAmount = 10.65,
            MinAmount = 100
        });

        #endregion
    }

    #endregion
}