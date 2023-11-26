using AppyNox.Services.Base.Application.ExceptionExtensions;
using AppyNox.Services.Base.Domain.Common.HttpStatusCodes;
using AppyNox.Services.Base.Domain.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace AppyNox.Services.Base.Application.Helpers
{
    public static class NoxEnumExtensions
    {
        #region [ Public Methods ]

        public static Enum GetEnumValueFromDisplayName(Type enumType, string displayName)
        {
            foreach (var field in enumType.GetFields())
            {
                if (Attribute.GetCustomAttribute(field, typeof(DisplayAttribute)) is DisplayAttribute attribute && attribute.Name == displayName)
                {
                    return (Enum)Enum.Parse(enumType, field.Name);
                }
            }
            throw new DtoDetailLevelNotFoundException(displayName, enumType);
        }

        public static string GetDisplayName(this Enum value)
        {
            return NoxEnumExtensionsBase.GetDisplayName(value);
        }

        #endregion
    }
}