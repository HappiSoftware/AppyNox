using AppyNox.Services.Base.Application.Dtos;

namespace AppyNox.Services.Authentication.Application.DTOs.IdentityUserDTOs.Models
{
    /// <summary>
    /// Data transfer object representing an identity user.
    /// </summary>
    public class ApplicationUserDto : DtoBase
    {
        #region [ Properties ]

        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        #endregion
    }
}