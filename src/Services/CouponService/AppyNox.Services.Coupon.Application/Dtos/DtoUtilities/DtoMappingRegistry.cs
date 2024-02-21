using AppyNox.Services.Base.Application.DtoUtilities;
using AppyNox.Services.Base.Application.Localization;
using AppyNox.Services.Base.Core.Enums;
using AppyNox.Services.Coupon.Application.Dtos.CouponDetailDtos.DetailLevel;
using AppyNox.Services.Coupon.Application.Dtos.CouponDetailTagDtos.DetailLevel;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.DetailLevel;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Base;
using AppyNox.Services.Coupon.Domain.Coupons;
using System.Data;
using System.Reflection;

namespace AppyNox.Services.Coupon.Application.Dtos.DtoUtilities
{
    public class DtoMappingRegistry : DtoMappingRegistryBase
    {
        #region [ Public Methods ]

        public DtoMappingRegistry()
            : base()
        {
            RegisterDtos();
        }

        public static Enum GetDetailLevel(Attribute attribute, DtoLevelMappingTypes mappingType)
        {
            if (attribute is CouponDetailLevelAttribute couponDetailAttribute)
            {
                return mappingType switch
                {
                    DtoLevelMappingTypes.DataAccess => couponDetailAttribute.DataAccessDetailLevel,
                    DtoLevelMappingTypes.Create => couponDetailAttribute.CreateDetailLevel,
                    DtoLevelMappingTypes.Update => couponDetailAttribute.UpdateDetailLevel,
                    _ => couponDetailAttribute.DataAccessDetailLevel
                };
            }
            else if (attribute is CouponDetailDetailLevelAttribute couponDetailDetailAttribute)
            {
                return mappingType switch
                {
                    DtoLevelMappingTypes.DataAccess => couponDetailDetailAttribute.DataAccessDetailLevel,
                    DtoLevelMappingTypes.Create => couponDetailDetailAttribute.CreateDetailLevel,
                    DtoLevelMappingTypes.Update => couponDetailDetailAttribute.UpdateDetailLevel,
                    _ => couponDetailDetailAttribute.DataAccessDetailLevel
                };
            }
            else if (attribute is CouponDetailTagDetailLevelAttribute couponDetailTagDetailAttribute)
            {
                return mappingType switch
                {
                    DtoLevelMappingTypes.DataAccess => couponDetailTagDetailAttribute.DataAccessDetailLevel,
                    DtoLevelMappingTypes.Create => couponDetailTagDetailAttribute.CreateDetailLevel,
                    DtoLevelMappingTypes.Update => couponDetailTagDetailAttribute.UpdateDetailLevel,
                    _ => couponDetailTagDetailAttribute.DataAccessDetailLevel
                };
            }

            throw new ArgumentException("Unsupported attribute type for detail level mapping.");
        }

        #endregion

        #region [ Protected Methods ]

        protected override void RegisterDtos()
        {
            var dtoTypes = Assembly.GetAssembly(typeof(CouponSimpleDto))?
                .GetTypes()
                .Where(t => t.Namespace != null && t.Namespace.Contains("Application.Dtos") && t.Namespace.Contains("Models"))
                .ToList();

            if (dtoTypes == null)
                return;

            foreach (var dtoType in dtoTypes)
            {
                var attributes = Attribute.GetCustomAttributes(dtoType);
                foreach (var attribute in attributes)
                {
                    if (attribute is CouponDetailLevelAttribute couponAttribute)
                    {
                        RegisterMapping(typeof(Domain.Coupons.Coupon), dtoType, couponAttribute);
                    }
                    else if (attribute is CouponDetailDetailLevelAttribute couponDetailAttribute)
                    {
                        RegisterMapping(typeof(CouponDetail), dtoType, couponDetailAttribute);
                    }
                    else if (attribute is CouponDetailTagDetailLevelAttribute couponDetailTagAttribute)
                    {
                        RegisterMapping(typeof(CouponDetailTag), dtoType, couponDetailTagAttribute);
                    }
                }
            }
        }

        protected override Enum GetDetailLevelBase(Attribute attribute, DtoLevelMappingTypes mappingType)
        {
            return GetDetailLevel(attribute, mappingType);
        }

        #endregion
    }
}