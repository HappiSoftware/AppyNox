﻿using AppyNox.Services.Base.Application.Dtos;

namespace AppyNox.Services.Authentication.Application.DTOs.ApplicationRoleDTOs.Models
{
    /// <summary>
    /// Data transfer object for updating an existing identity role.
    /// </summary>
    public class ApplicationRoleUpdateDto : ApplicationRoleCreateDto, IUpdateDto
    {
        #region [ Properties ]

        public Guid Id { get; set; }

        #endregion
    }
}