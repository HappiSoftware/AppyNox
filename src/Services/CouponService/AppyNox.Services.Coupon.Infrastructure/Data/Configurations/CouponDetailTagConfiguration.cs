using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppyNox.Services.Coupon.Domain.Coupons;

namespace AppyNox.Services.Coupon.Infrastructure.Data.Configurations;

internal class CouponDetailTagConfiguration(Guid detailId, Guid detailTagId) : IEntityTypeConfiguration<CouponDetailTag>
{
    #region [ Fields ]

    private readonly Guid _detailId = detailId;

    private readonly Guid _detailTagId = detailTagId;

    #endregion

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

        builder.HasOne(c => c.CouponDetailEntity)
            .WithMany(cd => cd.CouponDetailTags)
            .HasForeignKey(c => c.CouponDetailEntityId)
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
                Id = new CouponDetailTagId(_detailTagId),
                Tag = "Tag Description",
                CouponDetailEntityId = new CouponDetailId(_detailId),
                CreatedBy = "admin",
                CreationDate = DateTime.UtcNow,
                UpdatedBy = "admin",
                UpdateDate = DateTime.MinValue
            });

        #endregion
    }

    #endregion
}