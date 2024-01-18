using AppyNox.Services.Base.Application.Dtos;

namespace AppyNox.Services.Authentication.Application.DTOs.ApplicationUserDTOs.Models
{
    /// <summary>
    /// Data transfer object for updating an existing identity user.
    /// </summary>
    public class ApplicationUserUpdateDto : ApplicationUserCreateDto, IUpdateDto
    {
        #region [ Properties ]

        public Guid Id { get; set; }

        #endregion
    }
}