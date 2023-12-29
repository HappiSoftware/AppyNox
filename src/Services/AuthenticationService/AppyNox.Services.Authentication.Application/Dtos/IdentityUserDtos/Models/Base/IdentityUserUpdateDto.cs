namespace AppyNox.Services.Authentication.Application.Dtos.IdentityUserDtos.Models.Base
{
    public class IdentityUserUpdateDto : IdentityUserCreateDto
    {
        #region [ Properties ]

        public string Id { get; set; } = string.Empty;

        #endregion
    }
}