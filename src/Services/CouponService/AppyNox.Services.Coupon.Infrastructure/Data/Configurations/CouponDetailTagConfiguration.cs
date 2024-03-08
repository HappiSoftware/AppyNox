using AppyNox.Services.Coupon.Domain.Coupons;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppyNox.Services.Coupon.Infrastructure.Data.Configurations;

internal class CouponDetailTagConfiguration(CouponDetailId detailId, CouponDetailTagId detailTagId) : IEntityTypeConfiguration<CouponDetailTag>
{
    #region [ Public Methods ]

    public void Configure(EntityTypeBuilder<CouponDetailTag> builder)
    {
        #region [ Configurations ]

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .ValueGeneratedOnAdd();

        builder.Property(o => o.Id).HasConversion(
            couponId => couponId.Value,
            value => new CouponDetailTagId(value));

        builder.HasOne<CouponDetail>()
            .WithMany(cd => cd.CouponDetailTags)
            .HasForeignKey(c => c.CouponDetailId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.Tag).IsRequired();

        #endregion

        #region [ Seeds ]

        builder.HasData(
            new
            {
                Id = detailTagId,
                Tag = "Tag Description",
                CouponDetailId = detailId,
                CreatedBy = "System",
                CreationDate = DateTime.UtcNow,
                UpdatedBy = (string?)null,
                UpdateDate = (DateTime?)null
            });

        #endregion
    }

    #endregion
}