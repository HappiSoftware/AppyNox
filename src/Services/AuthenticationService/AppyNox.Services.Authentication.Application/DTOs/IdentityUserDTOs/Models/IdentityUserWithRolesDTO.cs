using AppyNox.Services.Authentication.Application.DTOs.IdentityRoleDTOs.Models;
using AppyNox.Services.Authentication.Application.DTOs.IdentityUserDTOs.DetailLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Authentication.Application.DTOs.IdentityUserDTOs.Models
{
    [IdentityUserDetailLevel(IdentityUserDetailLevel.WithRoles)]
    public class IdentityUserWithRolesDTO : IdentityUserDTO
    {
        public IList<IdentityRoleDTO>? Roles { get; set; }
    }
}
