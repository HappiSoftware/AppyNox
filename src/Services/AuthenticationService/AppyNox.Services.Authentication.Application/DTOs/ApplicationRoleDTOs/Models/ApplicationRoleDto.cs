using AppyNox.Services.Base.Application.Dtos;

namespace AppyNox.Services.Authentication.Application.DTOs.ApplicationRoleDTOs.Models
{
    /// <summary>
    /// Data transfer object representing an identity role.
    /// </summary>
    public class ApplicationRoleDto : DtoBase
    {
        #region [ Properties ]

        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        #endregion
    }
}