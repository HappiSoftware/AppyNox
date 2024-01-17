using System.ComponentModel.DataAnnotations;

namespace AppyNox.Services.License.Application.Dtos.ProductDtos.DetailLevel
{
    #region [ Enums ]

    public enum ProductDataAccessDetailLevel
    {
        [Display(Name = "Simple")]
        Simple
    }

    public enum ProductCreateDetailLevel
    {
        [Display(Name = "Simple")]
        Simple
    }

    public enum ProductUpdateDetailLevel
    {
        [Display(Name = "Simple")]
        Simple
    }

    #endregion

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ProductDetailLevelAttribute : Attribute
    {
        #region [ Public Constructors ]

        public ProductDetailLevelAttribute(ProductDataAccessDetailLevel dataAccessDetailLevel)
        {
            DataAccessDetailLevel = dataAccessDetailLevel;
        }

        public ProductDetailLevelAttribute(ProductCreateDetailLevel createDetailLevel)
        {
            CreateDetailLevel = createDetailLevel;
        }

        public ProductDetailLevelAttribute(ProductUpdateDetailLevel updateDetailLevel)
        {
            UpdateDetailLevel = updateDetailLevel;
        }

        #endregion

        #region [ Properties ]

        public ProductDataAccessDetailLevel DataAccessDetailLevel { get; }

        public ProductCreateDetailLevel CreateDetailLevel { get; }

        public ProductUpdateDetailLevel UpdateDetailLevel { get; }

        #endregion
    }
}