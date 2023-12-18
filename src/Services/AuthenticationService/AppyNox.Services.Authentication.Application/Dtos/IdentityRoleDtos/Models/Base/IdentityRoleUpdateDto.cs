using AppyNox.Services.Authentication.Application.Dtos.IdentityRoleDtos.DetailLevel;

namespace AppyNox.Services.Authentication.Application.Dtos.IdentityRoleDtos.Models.Base
{
    [IdentityRoleDetailLevel(IdentityRoleUpdateDetailLevel.Simple)]
    public class IdentityRoleUpdateDto : IdentityRoleCreateDto, IHasGuid
    {
        #region [ Properties ]

        public string Id { get; set; } = string.Empty;

        #endregion
    }
}