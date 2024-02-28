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

        builder.HasOne(c => c.CouponDetail)
            .WithMany(cd => cd.CouponDetailTags)
            .HasForeignKey(c => c.CouponDetailId)
            .IsRequired();

        builder.Property(x => x.Tag).IsRequired();

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
                Id = detailTagId,
                Tag = "Tag Description",
                CouponDetailId = detailId
            });
        builder.OwnsOne(a => a.Audit).HasData(new
        {
            CouponDetailTagId = detailTagId,
            CreatedBy = "admin",
            CreationDate = DateTime.UtcNow,
            UpdatedBy = string.Empty,
            UpdateDate = (DateTime?)null
        });

        #endregion
    }

    #endregion
}