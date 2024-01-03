﻿using AppyNox.Services.Authentication.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Authentication.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Configures the entity type CompanyEntity and seeds initial data.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the CompanyEntity class with the specified company ID.
    /// </remarks>
    /// <param name="companyId">The ID of the CompanyEntity for seeding data.</param>
    internal class CompanyConfiguration(Guid companyId) : IEntityTypeConfiguration<CompanyEntity>
    {
        #region [ Fields ]

        private readonly Guid _companyId = companyId;

        #endregion

        #region [ Public Methods ]

        /// <summary>
        /// Configures the CompanyEntity entity type and seeds data.
        /// </summary>
        /// <param name="builder">The builder being used to construct the entity type model.</param>
        public void Configure(EntityTypeBuilder<CompanyEntity> builder)
        {
            #region [ Configurations ]

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .ValueGeneratedOnAdd();

            builder.HasMany(c => c.Users)
                .WithOne(cd => cd.Company)
                .HasForeignKey(c => c.CompanyId)
                .IsRequired();

            builder.Property(x => x.Name).IsRequired();

            #endregion

            #region [ Seeds ]

            builder.HasData(
                new CompanyEntity
                {
                    Id = _companyId,
                    Name = "HappiSoft"
                });

            #endregion
        }

        #endregion
    }
}