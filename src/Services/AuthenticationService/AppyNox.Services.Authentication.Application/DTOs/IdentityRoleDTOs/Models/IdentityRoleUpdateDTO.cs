namespace AppyNox.Services.Authentication.Application.Dtos.IdentityRoleDtos.Models
{
    public class IdentityRoleUpdateDto : IdentityRoleCreateDto, IHasGuid
    {
        #region [ Properties ]

        public string Id { get; set; } = string.Empty;

        #endregion
    }
}