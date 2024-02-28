﻿using AppyNox.Services.Coupon.Domain.Coupons;
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
            .WithMany(cd => cd.Coupons)
            .HasForeignKey(c => c.CouponDetailId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

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

        builder.OwnsOne(o => o.Audit, auditableDataBuilder =>
        {
            auditableDataBuilder.Property(c => c.CreatedBy);
            auditableDataBuilder.Property(c => c.CreationDate);
            auditableDataBuilder.Property(c => c.UpdatedBy);
            auditableDataBuilder.Property(c => c.UpdateDate);
            auditableDataBuilder.WithOwner();
        });

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
            });
        builder.OwnsOne(o => o.Amount).HasData(new
        {
            CouponId = couponId1,
            DiscountAmount = 10.65,
            MinAmount = 100
        });
        builder.OwnsOne(a => a.Audit).HasData(new
        {
            CouponId = couponId1,
            CreatedBy = "admin",
            CreationDate = DateTime.UtcNow,
            UpdatedBy = string.Empty,
            UpdateDate = (DateTime?)null
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
            });
        builder.OwnsOne(o => o.Amount).HasData(new
        {
            CouponId = couponId2,
            DiscountAmount = 10.65,
            MinAmount = 100
        });
        builder.OwnsOne(a => a.Audit).HasData(new
        {
            CouponId = couponId2,
            CreatedBy = "admin",
            CreationDate = DateTime.UtcNow,
            UpdatedBy = string.Empty,
            UpdateDate = (DateTime?)null
        });

        #endregion
    }

    #endregion
}