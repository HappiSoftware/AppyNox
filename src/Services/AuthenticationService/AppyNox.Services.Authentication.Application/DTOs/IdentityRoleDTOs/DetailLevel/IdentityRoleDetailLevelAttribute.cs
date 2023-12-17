using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AppyNox.Services.Authentication.Application.Dtos.IdentityRoleDtos.DetailLevel
{
    #region [ Enums ]

    public enum IdentityRoleDataAccessDetailLevel
    {
        [Display(Name = "Simple")]
        Simple,
        
        [Display(Name = "WithAllProperties")]
        WithAllProperties,

        [Display(Name = "WithAllRelations")]
        WithAllRelations
    }

    public enum IdentityRoleCreateDetailLevel
    {
        [Display(Name = "Simple")]
        Simple,

        [Display(Name = "Extended")]
        Extended
    }

    public enum IdentityRoleUpdateDetailLevel
    {
        [Display(Name = "Simple")]
        Simple,

        [Display(Name = "Extended")]
        Extended
    }

    #endregion

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class IdentityRoleDetailLevelAttribute : Attribute
    {
        #region [ Public Constructors ]

        public IdentityRoleDetailLevelAttribute(IdentityRoleDataAccessDetailLevel dataAccessDetailLevel)
        {
            DataAccessDetailLevel = dataAccessDetailLevel;
        }

        public IdentityRoleDetailLevelAttribute(IdentityRoleCreateDetailLevel createDetailLevel)
        {
            CreateDetailLevel = createDetailLevel;
        }

        public IdentityRoleDetailLevelAttribute(IdentityRoleUpdateDetailLevel updateDetailLevel)
        {
            UpdateDetailLevel = updateDetailLevel;
        }

        #endregion

        #region [ Properties ]

        public IdentityRoleDataAccessDetailLevel DataAccessDetailLevel { get; }

        public IdentityRoleCreateDetailLevel CreateDetailLevel { get; }

        public IdentityRoleUpdateDetailLevel UpdateDetailLevel { get; }

        #endregion
    }
}