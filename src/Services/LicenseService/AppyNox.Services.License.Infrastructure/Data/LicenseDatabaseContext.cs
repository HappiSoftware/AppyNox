﻿using AppyNox.Services.Base.Application.Interfaces.Encryption;
using AppyNox.Services.Base.Infrastructure.Data;
using AppyNox.Services.License.Domain.Entities;
using AppyNox.Services.License.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace AppyNox.Services.License.Infrastructure.Data
{
    public class LicenseDatabaseContext(DbContextOptions<LicenseDatabaseContext> options, IEncryptionService? encryptionService = null) 
        : NoxDatabaseContext(options, encryptionService)
    {

        #region [ Public Constructors ]

        #endregion

        #region [ Properties ]

        public DbSet<LicenseEntity> Licenses { get; set; }

        public DbSet<ApplicationUserLicenses> ApplicationUserLicenses { get; set; }

        public DbSet<ApplicationUserLicenseMacAddress> ApplicationUserLicenseMacAddresses { get; set; }

        public DbSet<ProductEntity> Products { get; set; }

        #endregion

        #region [ Protected Methods ]

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            //optionsBuilder.UseNpgsql("User ID=postgres;Password=sapass;Server=localhost;Port=5432;Database=AppyNox_License;Pooling=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region [ Common Ids ]

            Guid licenseId = Guid.Parse("00455e60-8524-48df-955c-cc9b1f2e7476");
            Guid productId = Guid.Parse("9991492a-118c-4f20-ac8c-76410d57957c");

            #endregion

            #region [ Entity Configurations ]

            modelBuilder.ApplyConfiguration(new ProductConfiguration(productId));
            modelBuilder.ApplyConfiguration(new LicenseConfiguration(licenseId, productId));
            modelBuilder.ApplyConfiguration(new ApplicationUserLicensesConfiguration());
            modelBuilder.ApplyConfiguration(new ApplicationUserLicenseMacAddressConfiguration());

            #endregion
        }

        #endregion
    }
}