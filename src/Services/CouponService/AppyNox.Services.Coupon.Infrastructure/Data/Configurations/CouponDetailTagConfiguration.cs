using AppyNox.Services.Coupon.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        builder.HasOne(c => c.CouponDetailEntity)
            .WithMany(cd => cd.CouponDetailTags)
            .HasForeignKey(c => c.CouponDetailEntityId)
            .IsRequired();

        builder.Property(x => x.Tag).IsRequired();

        #endregion

        #region [ Seeds ]

        builder.HasData(
            new CouponDetailTag
            {
                Id = _detailTagId,
                Tag = "Tag Description",
                CouponDetailEntityId = _detailId
            });

        #endregion
    }

    #endregion
}