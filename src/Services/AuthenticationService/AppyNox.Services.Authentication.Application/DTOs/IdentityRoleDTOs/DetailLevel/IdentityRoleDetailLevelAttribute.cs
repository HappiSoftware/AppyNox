using System.ComponentModel;

namespace AppyNox.Services.Authentication.Application.DTOs.IdentityRoleDTOs.DetailLevel
{
    public enum IdentityRoleDetailLevel
    {
        [Description("Basic")]
        Basic,

        [Description("WithClaims")]
        WithClaims,

        [Description("WithAllProperties")]
        WithAllProperties
    }

    public class IdentityRoleDetailLevelAttribute : Attribute
    {
        #region [ Public Constructors ]

        public IdentityRoleDetailLevelAttribute(IdentityRoleDetailLevel level)
        {
            DetailLevel = level;
        }

        #endregion

        #region [ Properties ]

        public IdentityRoleDetailLevel DetailLevel { get; }

        #endregion
    }
}