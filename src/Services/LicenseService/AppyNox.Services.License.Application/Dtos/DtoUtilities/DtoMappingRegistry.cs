using AppyNox.Services.Base.Application.DtoUtilities;
using AppyNox.Services.Base.Application.ExceptionExtensions.Base;
using AppyNox.Services.Base.Application.Localization;
using AppyNox.Services.Base.Core.Enums;
using AppyNox.Services.License.Application.Dtos.LicenseDtos.DetailLevel;
using AppyNox.Services.License.Application.Dtos.LicenseDtos.Models.Base;
using AppyNox.Services.License.Application.Dtos.ProductDtos.DetailLevel;
using AppyNox.Services.License.Domain.Entities;
using System.Data;
using System.Net;
using System.Reflection;

namespace AppyNox.Services.License.Application.Dtos.DtoUtilities
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
            if (attribute is LicenseDetailLevelAttribute licenseDetailAttribute)
            {
                return mappingType switch
                {
                    DtoLevelMappingTypes.DataAccess => licenseDetailAttribute.DataAccessDetailLevel,
                    DtoLevelMappingTypes.Create => licenseDetailAttribute.CreateDetailLevel,
                    DtoLevelMappingTypes.Update => licenseDetailAttribute.UpdateDetailLevel,
                    _ => licenseDetailAttribute.DataAccessDetailLevel
                };
            }
            else if (attribute is ProductDetailLevelAttribute productDetailAttribute)
            {
                return mappingType switch
                {
                    DtoLevelMappingTypes.DataAccess => productDetailAttribute.DataAccessDetailLevel,
                    DtoLevelMappingTypes.Create => productDetailAttribute.CreateDetailLevel,
                    DtoLevelMappingTypes.Update => productDetailAttribute.UpdateDetailLevel,
                    _ => productDetailAttribute.DataAccessDetailLevel
                };
            }

            throw new NoxApplicationException("Unsupported attribute type for detail level mapping. Check DtoMappingRegistry",
                (int)HttpStatusCode.InternalServerError);
        }

        #endregion

        #region [ Protected Methods ]

        protected override void RegisterDtos()
        {
            var dtoTypes = Assembly.GetAssembly(typeof(LicenseSimpleDto))?
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
                    if (attribute is LicenseDetailLevelAttribute licenseAttribute)
                    {
                        RegisterMapping(typeof(LicenseEntity), dtoType, licenseAttribute);
                    }
                    else if (attribute is ProductDetailLevelAttribute productAttribute)
                    {
                        RegisterMapping(typeof(ProductEntity), dtoType, productAttribute);
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