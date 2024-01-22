using AppyNox.Services.Base.Application.Dtos;

namespace AppyNox.Services.Authentication.Application.DTOs.ApplicationUserDTOs.Models
{
    /// <summary>
    /// Data transfer object representing an identity user.
    /// </summary>
    public class ApplicationUserDto : DtoBase
    {
        #region [ Properties ]

        public Guid Id { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        #endregion
    }
}