using AppyNox.Services.Base.Application.Dtos;

namespace AppyNox.Services.Sso.Application.DTOs.ApplicationRoleDTOs.Models
{
    /// <summary>
    /// Data transfer object for updating an existing identity role.
    /// </summary>
    public class ApplicationRoleUpdateDto : ApplicationRoleCreateDto
    {
        #region [ Properties ]

        public Guid Id { get; set; }

        #endregion
    }
}