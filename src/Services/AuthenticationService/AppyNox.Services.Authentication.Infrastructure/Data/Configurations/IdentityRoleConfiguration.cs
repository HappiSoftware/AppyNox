using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppyNox.Services.Authentication.Infrastructure.Data.Configurations
{
    public class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        private readonly string _adminRoleId;
        public IdentityRoleConfiguration(string adminRoleId)
        {
            _adminRoleId = adminRoleId;
        }
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            #region [ Configurations ]
            #endregion

            #region [ Seeds ]

            builder.HasData(new IdentityRole 
            { 
                Id = _adminRoleId, 
                Name = "Admin", 
                NormalizedName = "ADMIN" 
            });

            #endregion
        }
    }
}
