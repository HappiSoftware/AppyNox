using AppyNox.Services.License.Domain.Entities;
using AppyNox.Services.License.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace AppyNox.Services.License.Infrastructure.Data
{
    public class LicenseDatabaseContext : DbContext
    {
        #region [ Public Constructors ]

        public LicenseDatabaseContext()
        {
        }

        public LicenseDatabaseContext(DbContextOptions<LicenseDatabaseContext> options)
            : base(options)
        {
        }

        #endregion

        #region [ Properties ]

        public DbSet<LicenseEntity> Licenses { get; set; }

        #endregion

        #region [ Protected Methods ]

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region [ Common Ids ]

            Guid licenseId = Guid.Parse("00455e60-8524-48df-955c-cc9b1f2e7476");

            #endregion

            #region [ Entity Configurations ]

            modelBuilder.ApplyConfiguration(new LicenseConfiguration(licenseId));

            #endregion
        }

        #endregion
    }
}