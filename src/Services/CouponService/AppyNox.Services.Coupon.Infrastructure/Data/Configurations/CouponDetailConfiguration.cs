using AppyNox.Services.Coupon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppyNox.Services.Coupon.Infrastructure.Data.Configurations
{
    public class CouponDetailConfiguration : IEntityTypeConfiguration<CouponDetailEntity>
    {
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

            #endregion

            #region [ Seeds ]

            builder.HasData(new CouponDetailEntity
            {
                Id = Guid.Parse("c2feaca4-d82a-4d2e-ba5a-667b685212b4"),
                Code = "EXF50",
                Detail = "TestDetail"
            });

            #endregion
        }

        #endregion
    }
}