using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Authentication.Application.DTOs.IdentityRoleDTOs.DetailLevel
{
    public class IdentityRoleDetailLevelAttribute : Attribute
    {
        public IdentityRoleDetailLevel DetailLevel { get; }

        public IdentityRoleDetailLevelAttribute(IdentityRoleDetailLevel level)
        {
            DetailLevel = level;
        }

    }

    public enum IdentityRoleDetailLevel
    {
        [Description("Basic")]
        Basic,

        [Description("WithClaims")]
        WithClaims,

        [Description("WithAllProperties")]
        WithAllProperties
    }
}
