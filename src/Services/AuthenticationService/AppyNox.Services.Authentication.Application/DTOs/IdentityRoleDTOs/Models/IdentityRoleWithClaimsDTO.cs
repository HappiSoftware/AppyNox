using AppyNox.Services.Authentication.Application.DTOs.ClaimDTOs;
using AppyNox.Services.Authentication.Application.DTOs.IdentityRoleDTOs.DetailLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Authentication.Application.DTOs.IdentityRoleDTOs.Models
{
    [IdentityRoleDetailLevel(IdentityRoleDetailLevel.WithClaims)]
    public class IdentityRoleWithClaimsDTO : IdentityRoleDTO
    {
        public IList<ClaimDTO>? Claims { get; set; }
    }
}
