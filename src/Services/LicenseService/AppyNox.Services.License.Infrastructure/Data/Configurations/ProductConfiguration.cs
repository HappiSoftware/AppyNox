﻿using AppyNox.Services.License.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppyNox.Services.License.Infrastructure.Data.Configurations
{
    internal class ProductConfiguration(Guid productId) : IEntityTypeConfiguration<ProductEntity>
    {
        #region [ Fields ]

        private readonly Guid _productId = productId;

        #endregion

        #region [ Public Methods ]

        public void Configure(EntityTypeBuilder<ProductEntity> builder)
        {
            #region [ Configurations ]

            builder.HasKey(c => c.Id);

            builder.Property(o => o.Id).HasConversion(
            productId => productId.Value,
            value => new ProductId(value));

            builder.HasMany(p => p.Licenses)
                .WithOne(l => l.Product)
                .HasForeignKey(l => l.ProductId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Code).IsRequired().HasMaxLength(5);
            builder.Property(x => x.Name).HasMaxLength(20).IsUnicode().IsRequired();

            #endregion

            #region [ Seeds ]

            builder.HasData(
                new
                {
                    Id = new ProductId(_productId),
                    Code = "PROD1",
                    Name = "AppyNox",
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