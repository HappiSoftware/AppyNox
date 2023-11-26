using AppyNox.Services.Base.Domain.Common.HttpStatusCodes;
using AppyNox.Services.Base.Domain.ExceptionExtensions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace AppyNox.Services.Base.Domain.Helpers
{
    public static class NoxEnumExtensionsBase
    {
        #region [ Public Methods ]

        public static string GetDisplayName(this Enum value)
        {
            var attribute = value.GetAttributeOfType<DisplayAttribute>();

            return attribute?.Name ?? throw new EnumDisplayNameNotFoundException(value);
        }

        #endregion

        #region Private Methods

        private static T GetAttributeOfType<T>(this Enum enumValue) where T : Attribute
        {
            var type = enumValue.GetType();
            var memInfo = type.GetMember(enumValue.ToString()).First();
            var attributes = memInfo.GetCustomAttributes<T>(false);
            return attributes.FirstOrDefault() ?? throw new EnumDisplayNameNotFoundException(enumValue);
        }

        #endregion
    }
}