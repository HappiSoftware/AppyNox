using AppyNox.Services.Base.Application.Dtos;

namespace AppyNox.Services.Authentication.Application.DTOs.ApplicationRoleDTOs.Models
{
    /// <summary>
    /// Data transfer object for creating a new identity role.
    /// </summary>
    public class ApplicationRoleCreateDto : DtoBase
    {
        #region [ Properties ]

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        #endregion
    }
}