using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppyNox.Services.Authentication.Infrastructure.Data.Configurations
{
    public class IdentityUserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        private readonly string _adminRoleId;
        private readonly string _adminUserId;
        public IdentityUserRoleConfiguration(string adminRoleId, string adminUserId)
        {
            _adminRoleId = adminRoleId;
            _adminUserId = adminUserId;

        }
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            #region [ Configurations ]
            #endregion

            #region [ Seeds ]

            builder.HasData(
                new IdentityUserRole<string> { RoleId = _adminRoleId, UserId = _adminUserId }
            );

            #endregion
        }
    }
}
