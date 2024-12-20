﻿using AppyNox.Services.Base.Application.Dtos;

namespace AppyNox.Services.Sso.Application.DTOs.ApplicationUserDTOs.Models
{
    /// <summary>
    /// Data transfer object for updating an existing identity user.
    /// </summary>
    public class ApplicationUserUpdateDto : ApplicationUserCreateDto
    {
        #region [ Properties ]

        public Guid Id { get; set; }

        #endregion
    }
}