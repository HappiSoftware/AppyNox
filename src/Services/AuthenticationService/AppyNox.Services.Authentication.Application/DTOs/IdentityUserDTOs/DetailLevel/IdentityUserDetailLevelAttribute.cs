using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Authentication.Application.DTOs.IdentityUserDTOs.DetailLevel
{
    public class IdentityUserDetailLevelAttribute : Attribute
    {
        public IdentityUserDetailLevel DetailLevel { get; }

        public IdentityUserDetailLevelAttribute(IdentityUserDetailLevel level)
        {
            DetailLevel = level;
        }

    }

    public enum IdentityUserDetailLevel
    {
        [Description("Basic")]
        Basic,

        [Description("WithRoles")]
        WithRoles,

        [Description("WithAllProperties")]
        WithAllProperties
    }
}
