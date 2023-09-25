using AppyNox.Services.Authentication.Application.DTOs.ClaimDTOs;
using AppyNox.Services.Authentication.Application.DTOs.IdentityRoleDTOs.DetailLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Authentication.Application.DTOs.IdentityRoleDTOs.Models
{
    [IdentityRoleDetailLevel(IdentityRoleDetailLevel.WithAllProperties)]
    internal class IdentityRoleWithAllPropertiesDTO : IdentityRoleDTO
    {
        public string Id { get; set; } = string.Empty;
        public IList<ClaimDTO>? Claims { get; set; }
    }
}
