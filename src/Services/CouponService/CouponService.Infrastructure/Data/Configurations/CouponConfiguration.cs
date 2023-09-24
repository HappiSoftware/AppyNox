using CouponService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponService.Infrastructure.Data.Configurations
{
    public class CouponConfiguration : IEntityTypeConfiguration<CouponEntity>
    {
        public void Configure(EntityTypeBuilder<CouponEntity> builder)
        {
            builder.Property(x => x.MinAmount).IsRequired();
            builder.Property(x => x.Code).IsRequired().HasMaxLength(5);
            builder.Property(x => x.Description).HasMaxLength(60).IsUnicode();
        }
    }
}
