using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppyNox.Services.Authentication.Infrastructure.Data.Configurations
{
    public class IdentityUserConfiguration : IEntityTypeConfiguration<IdentityUser>
    {
        private readonly string _adminUserId;
        public IdentityUserConfiguration(string adminUserId)
        {
            _adminUserId = adminUserId;
        }
        public void Configure(EntityTypeBuilder<IdentityUser> builder)
        {
            #region [ Configurations ]
            #endregion

            #region [ Seeds ]

            var hasher = new PasswordHasher<IdentityUser>();

            builder.HasData(new IdentityUser 
            { 
                Id = _adminUserId,
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@email.com",
                NormalizedEmail = "ADMIN@EMAIL.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(new IdentityUser(), "Admin@123")
            });

            #endregion
        }
    }
}
