using AppyNox.Services.Coupon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppyNox.Services.Coupon.Infrastructure.Data.Configurations
{
    internal class CouponConfiguration(Guid couponId1, Guid couponId2, Guid couponDetailId) : IEntityTypeConfiguration<CouponEntity>
    {
        #region [ Fields ]

        private readonly Guid _couponId1 = couponId1;

        private readonly Guid _couponId2 = couponId2;

        private readonly Guid _couponDetailId = couponDetailId;

        #endregion

        #region [ Public Methods ]

        public void Configure(EntityTypeBuilder<CouponEntity> builder)
        {
            #region [ Configurations ]

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .ValueGeneratedOnAdd();

            builder.HasOne(c => c.CouponDetailEntity)
                .WithMany(cd => cd.Coupons)
                .HasForeignKey(c => c.CouponDetailEntityId)
                .IsRequired();
            builder.Property(x => x.MinAmount).IsRequired();
            builder.Property(x => x.Code).IsRequired().HasMaxLength(5);
            builder.Property(x => x.Description).HasMaxLength(60).IsUnicode().IsRequired();
            builder.Property(x => x.Detail).HasMaxLength(60).IsUnicode();

            #endregion

            #region [ Seeds ]

            builder.HasData(
                new CouponEntity
                {
                    Id = _couponId1,
                    Code = "EXF50",
                    Description = "Description",
                    DiscountAmount = 10.65,
                    MinAmount = 100,
                    Detail = "Detail1",
                    CouponDetailEntityId = _couponDetailId
                });

            builder.HasData(
                new CouponEntity
                {
                    Id = _couponId2,
                    Code = "EXF60",
                    Description = "Description2",
                    DiscountAmount = 20.55,
                    MinAmount = 200,
                    Detail = "Detail2",
                    CouponDetailEntityId = _couponDetailId
                });

            #endregion
        }

        #endregion
    }
}