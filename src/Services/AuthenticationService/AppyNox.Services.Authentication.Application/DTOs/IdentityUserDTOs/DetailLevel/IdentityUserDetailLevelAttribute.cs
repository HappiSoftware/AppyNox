using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AppyNox.Services.Authentication.Application.Dtos.IdentityUserDtos.DetailLevel
{
    #region [ Enums ]

    public enum IdentityUserDataAccessDetailLevel
    {
        [Display(Name = "Simple")]
        Simple,
        
        [Display(Name = "WithAllProperties")]
        WithAllProperties,

        [Display(Name = "WithAllRelations")]
        WithAllRelations
    }

    public enum IdentityUserCreateDetailLevel
    {
        [Display(Name = "Simple")]
        Simple,

        [Display(Name = "Extended")]
        Extended
    }

    public enum IdentityUserUpdateDetailLevel
    {
        [Display(Name = "Simple")]
        Simple,

        [Display(Name = "Extended")]
        Extended
    }

    #endregion

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class IdentityUserDetailLevelAttribute : Attribute
    {
        #region [ Public Constructors ]

        public IdentityUserDetailLevelAttribute(IdentityUserDataAccessDetailLevel dataAccessDetailLevel)
        {
            DataAccessDetailLevel = dataAccessDetailLevel;
        }

        public IdentityUserDetailLevelAttribute(IdentityUserCreateDetailLevel createDetailLevel)
        {
            CreateDetailLevel = createDetailLevel;
        }

        public IdentityUserDetailLevelAttribute(IdentityUserUpdateDetailLevel updateDetailLevel)
        {
            UpdateDetailLevel = updateDetailLevel;
        }

        #endregion

        #region [ Properties ]

        public IdentityUserDataAccessDetailLevel DataAccessDetailLevel { get; }

        public IdentityUserCreateDetailLevel CreateDetailLevel { get; }

        public IdentityUserUpdateDetailLevel UpdateDetailLevel { get; }

        #endregion
    }
}