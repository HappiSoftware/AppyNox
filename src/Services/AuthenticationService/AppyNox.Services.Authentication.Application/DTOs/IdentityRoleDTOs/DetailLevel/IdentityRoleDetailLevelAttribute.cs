using System.ComponentModel;

namespace AppyNox.Services.Authentication.Application.Dtos.IdentityRoleDtos.DetailLevel
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

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
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