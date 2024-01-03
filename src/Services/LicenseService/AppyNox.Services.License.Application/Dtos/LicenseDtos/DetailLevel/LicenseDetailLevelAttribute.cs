using System.ComponentModel.DataAnnotations;

namespace AppyNox.Services.License.Application.Dtos.LicenseDtos.DetailLevel
{
    #region [ Enums ]

    public enum LicenseDataAccessDetailLevel
    {
        [Display(Name = "Simple")]
        Simple
    }

    public enum LicenseCreateDetailLevel
    {
        [Display(Name = "Simple")]
        Simple
    }

    public enum LicenseUpdateDetailLevel
    {
        [Display(Name = "Simple")]
        Simple
    }

    #endregion

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class LicenseDetailLevelAttribute : Attribute
    {
        #region [ Public Constructors ]

        public LicenseDetailLevelAttribute(LicenseDataAccessDetailLevel dataAccessDetailLevel)
        {
            DataAccessDetailLevel = dataAccessDetailLevel;
        }

        public LicenseDetailLevelAttribute(LicenseCreateDetailLevel createDetailLevel)
        {
            CreateDetailLevel = createDetailLevel;
        }

        public LicenseDetailLevelAttribute(LicenseUpdateDetailLevel updateDetailLevel)
        {
            UpdateDetailLevel = updateDetailLevel;
        }

        #endregion

        #region [ Properties ]

        public LicenseDataAccessDetailLevel DataAccessDetailLevel { get; }

        public LicenseCreateDetailLevel CreateDetailLevel { get; }

        public LicenseUpdateDetailLevel UpdateDetailLevel { get; }

        #endregion
    }
}