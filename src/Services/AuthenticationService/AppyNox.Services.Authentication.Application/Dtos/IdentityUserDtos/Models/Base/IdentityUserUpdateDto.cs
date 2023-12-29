namespace AppyNox.Services.Authentication.Application.Dtos.IdentityUserDtos.Models.Base
{
    /// <summary>
    /// Data transfer object for updating an existing identity user.
    /// </summary>
    public class IdentityUserUpdateDto : IdentityUserCreateDto
    {
        #region [ Properties ]

        public string Id { get; set; } = string.Empty;

        #endregion
    }
}