using AppyNox.Services.Coupon.Domain.Coupons;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace AppyNox.Services.Coupon.Infrastructure.Data.Configurations;

internal class CouponConfiguration(Guid couponId1, Guid couponId2, Guid couponDetailId) : IEntityTypeConfiguration<Domain.Coupons.Coupon>
{
    #region [ Fields ]

    private readonly Guid _couponId1 = couponId1;

    private readonly Guid _couponId2 = couponId2;

    private readonly Guid _couponDetailId = couponDetailId;

    #endregion

    #region [ Public Methods ]

    public void Configure(EntityTypeBuilder<Domain.Coupons.Coupon> builder)
    {
        #region [ Configurations ]

        builder.HasKey(c => c.Id);

        builder.Property(o => o.Id).HasConversion(
            couponId => couponId.Value,
            value => new CouponId(value));

        builder.HasOne(c => c.CouponDetailEntity)
            .WithMany(cd => cd.Coupons)
            .HasForeignKey(c => c.CouponDetailEntityId)
            .IsRequired();
        builder.Navigation(b => b.CouponDetailEntity).IsRequired();

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
                Id = new CouponId(_couponId1),
                Code = "EXF50",
                Description = "Description",
                DiscountAmount = 10.65,
                MinAmount = 100,
                Detail = "Detail1",
                CouponDetailEntityId = new CouponDetailId(_couponDetailId),
                CreatedBy = "admin",
                CreationDate = DateTime.UtcNow,
                UpdatedBy = "admin",
                UpdateDate = DateTime.MinValue
            });

        builder.HasData(
            new
            {
                Id = new CouponId(_couponId2),
                Code = "EXF60",
                Description = "Description2",
                DiscountAmount = 20.55,
                MinAmount = 200,
                Detail = "Detail2",
                CouponDetailEntityId = new CouponDetailId(_couponDetailId),
                CreatedBy = "admin",
                CreationDate = DateTime.UtcNow,
                UpdatedBy = "admin",
                UpdateDate = DateTime.MinValue
            });

        #endregion
    }

    #endregion
}