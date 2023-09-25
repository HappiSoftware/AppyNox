using AppyNox.Services.Authentication.Application.DTOs.IdentityRoleDTOs.DetailLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Authentication.Application.DTOs.IdentityRoleDTOs.Models
{
    [IdentityRoleDetailLevel(IdentityRoleDetailLevel.Basic)]
    public class IdentityRoleDTO
    {
        public string Name { get; set; } = string.Empty;
    }
}
