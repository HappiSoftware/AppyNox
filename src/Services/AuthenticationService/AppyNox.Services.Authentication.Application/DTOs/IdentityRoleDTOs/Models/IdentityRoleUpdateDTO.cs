namespace AppyNox.Services.Authentication.Application.DTOs.IdentityRoleDTOs.Models
{
    public class IdentityRoleUpdateDTO : IdentityRoleCreateDTO, IHasGuid
    {
        #region [ Properties ]

        public string Id { get; set; } = string.Empty;

        #endregion
    }
}