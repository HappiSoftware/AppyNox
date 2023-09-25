using AppyNox.Services.Authentication.Application.DTOs.IdentityUserDTOs.DetailLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Authentication.Application.DTOs.IdentityUserDTOs.Models
{
    [IdentityUserDetailLevel(IdentityUserDetailLevel.WithAllProperties)]
    public class IdentityUserWithAllPropertiesDTO : IdentityUserWithRolesDTO
    {
        public string Id { get; set; } = string.Empty;
    }
}
