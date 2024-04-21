using AppyNox.Services.Coupon.Domain.Coupons;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppyNox.Services.Coupon.Infrastructure.Data.Configurations
{
    internal class CouponDetailConfiguration(CouponDetailId couponDetailId) : IEntityTypeConfiguration<CouponDetail>
    {
        #region [ Public Methods ]

        public void Configure(EntityTypeBuilder<CouponDetail> builder)
        {
            #region [ Configurations ]

            builder.HasKey(cd => cd.Id);

            builder.Property(cd => cd.Id)
                .ValueGeneratedOnAdd();

            builder.Property(o => o.Id).HasConversion(
                couponId => couponId.Value,
                value => new CouponDetailId(value));

            #endregion

            #region [ Seeds ]

            builder.HasData(
               new
               {
                   Id = couponDetailId,
                   Code = "EXD10",
                   Detail = "TestDetail",
                   CreatedBy = "System",
                   CreationDate = DateTime.SpecifyKind(new DateTime(2024, 4, 21), DateTimeKind.Utc),
                   UpdatedBy = (string?)null,
                   UpdateDate = (DateTime?)null
               });

            #endregion
        }

        #endregion
    }
}