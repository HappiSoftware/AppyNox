using AppyNox.Services.Coupon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppyNox.Services.Coupon.Infrastructure.Data.Configurations
{
    public class CouponConfiguration : IEntityTypeConfiguration<CouponEntity>
    {
        #region [ Public Methods ]

        public void Configure(EntityTypeBuilder<CouponEntity> builder)
        {
            #region [ Configurations ]

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .ValueGeneratedOnAdd();

            builder.HasOne(c => c.CouponDetailEntity)
                .WithMany(cd => cd.Coupons)
                .HasForeignKey(c => c.CouponDetailEntityId)
                .IsRequired();
            builder.Property(x => x.MinAmount).IsRequired();
            builder.Property(x => x.Code).IsRequired().HasMaxLength(5);
            builder.Property(x => x.Description).HasMaxLength(60).IsUnicode().IsRequired();
            builder.Property(x => x.Detail).HasMaxLength(60).IsUnicode();

            #endregion

            #region [ Seeds ]

            builder.HasData(
                new CouponEntity
                {
                    Id = Guid.NewGuid(),
                    Code = "EXF50",
                    Description = "Description",
                    DiscountAmount = 10.65,
                    MinAmount = 100,
                    Detail = "Detail1",
                    CouponDetailEntityId = Guid.Parse("c2feaca4-d82a-4d2e-ba5a-667b685212b4")
                });

            builder.HasData(
                new CouponEntity
                {
                    Id = Guid.NewGuid(),
                    Code = "EXF60",
                    Description = "Description2",
                    DiscountAmount = 20.55,
                    MinAmount = 200,
                    Detail = "Detail2",
                    CouponDetailEntityId = Guid.Parse("c2feaca4-d82a-4d2e-ba5a-667b685212b4")
                });

            #endregion
        }

        #endregion
    }
}