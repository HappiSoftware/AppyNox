namespace AppyNox.Services.Authentication.Application.Dtos.IdentityRoleDtos.Models.Base
{
    /// <summary>
    /// Data transfer object for updating an existing identity role.
    /// </summary>
    public class IdentityRoleUpdateDto : IdentityRoleCreateDto, IHasGuid
    {
        #region [ Properties ]

        public string Id { get; set; } = string.Empty;

        #endregion
    }
}