using AppyNox.Services.Coupon.Domain.Coupons;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppyNox.Services.Coupon.Infrastructure.Data.Configurations
{
    internal class CouponDetailConfiguration(Guid couponDetailId) : IEntityTypeConfiguration<CouponDetail>
    {
        #region [ Fields ]

        private readonly Guid _couponDetailId = couponDetailId;

        #endregion

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
                   Id = new CouponDetailId(couponDetailId),
                   Code = "EXD10",
                   Detail = "TestDetail",
                   CreatedBy = "admin",
                   CreationDate = DateTime.UtcNow,
                   UpdatedBy = "admin",
                   UpdateDate = DateTime.MinValue
               });

            #endregion
        }

        #endregion
    }
}