using System.ComponentModel;

namespace AppyNox.Services.Authentication.Application.DTOs.IdentityUserDTOs.DetailLevel
{
    public enum IdentityUserDetailLevel
    {
        [Description("Basic")]
        Basic,

        [Description("WithRoles")]
        WithRoles,

        [Description("WithAllProperties")]
        WithAllProperties
    }

    public class IdentityUserDetailLevelAttribute : Attribute
    {
        #region [ Public Constructors ]

        public IdentityUserDetailLevelAttribute(IdentityUserDetailLevel level)
        {
            DetailLevel = level;
        }

        #endregion

        #region [ Properties ]

        public IdentityUserDetailLevel DetailLevel { get; }

        #endregion
    }
}