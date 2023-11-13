using System.ComponentModel;

namespace AppyNox.Services.Authentication.Application.Dtos.IdentityUserDtos.DetailLevel
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

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
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