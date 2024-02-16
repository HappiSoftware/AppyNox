using AppyNox.Services.Coupon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppyNox.Services.Coupon.Infrastructure.Data.Configurations
{
    internal class CouponDetailConfiguration(Guid couponDetailId) : IEntityTypeConfiguration<CouponDetailEntity>
    {
        #region [ Fields ]

        private readonly Guid _couponDetailId = couponDetailId;

        #endregion

        #region [ Public Methods ]

        public void Configure(EntityTypeBuilder<CouponDetailEntity> builder)
        {
            #region [ Configurations ]

            builder.HasKey(cd => cd.Id);

            builder.Property(cd => cd.Id)
                .ValueGeneratedOnAdd();

            builder.HasMany(cd => cd.Coupons)
                .WithOne(c => c.CouponDetailEntity)
                .HasForeignKey(c => c.CouponDetailEntityId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(cd => cd.CouponDetailTags)
                .WithOne(c => c.CouponDetailEntity)
                .HasForeignKey(c => c.CouponDetailEntityId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region [ Seeds ]

            builder.HasData(new CouponDetailEntity
            {
                Id = _couponDetailId,
                Code = "EXF50",
                Detail = "TestDetail"
            });

            #endregion
        }

        #endregion
    }
}