using AppyNox.Services.Base.Application.DtoUtilities;
using AppyNox.Services.Base.Application.Localization;
using AppyNox.Services.Base.Core.Enums;
using AppyNox.Services.Coupon.Application.Dtos.CouponDetailDtos.DetailLevel;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.DetailLevel;
using AppyNox.Services.Coupon.Application.Dtos.CouponDtos.Models.Base;
using AppyNox.Services.Coupon.Domain.Entities;
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
                        RegisterMapping(typeof(CouponEntity), dtoType, couponAttribute);
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