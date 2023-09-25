using AppyNox.Services.Authentication.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Authentication.Application.DTOs.IdentityRoleDTOs.Models
{
    public class IdentityRoleUpdateDTO : IdentityRoleCreateDTO, IHasGuid
    {
        public string Id { get; set; } = string.Empty;
    }
}
