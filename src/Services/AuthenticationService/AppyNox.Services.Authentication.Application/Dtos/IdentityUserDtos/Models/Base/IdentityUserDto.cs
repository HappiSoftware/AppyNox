﻿using AppyNox.Services.Base.Application.Dtos;

namespace AppyNox.Services.Authentication.Application.Dtos.IdentityUserDtos.Models.Base
{
    public class IdentityUserDto : BaseDto
    {
        #region [ Properties ]

        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        #endregion
    }
}