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
                   Id = couponDetailId,
                   Code = "EXD10",
                   Detail = "TestDetail"
               });
            builder.OwnsOne(a => a.Audit).HasData(new
            {
                CouponDetailId = couponDetailId,
                CreatedBy = "admin",
                CreationDate = DateTime.UtcNow,
                UpdatedBy = string.Empty,
                UpdateDate = (DateTime?)null
            });

            #endregion
        }

        #endregion
    }
}