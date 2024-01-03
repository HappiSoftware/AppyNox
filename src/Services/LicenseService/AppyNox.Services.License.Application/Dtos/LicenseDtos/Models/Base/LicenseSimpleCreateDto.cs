﻿using AppyNox.Services.Base.Application.Dtos;
using AppyNox.Services.License.Application.Dtos.LicenseDtos.DetailLevel;

namespace AppyNox.Services.License.Application.Dtos.LicenseDtos.Models.Base
{
    [LicenseDetailLevel(LicenseCreateDetailLevel.Simple)]
    public class LicenseSimpleCreateDto : BaseDto
    {
        #region [ Properties ]

        public string Description { get; set; } = string.Empty;

        public string LicenseKey { get; set; } = string.Empty;

        public DateTime ExpirationDate { get; set; }

        public int MaxUsers { get; set; }

        #endregion
    }
}