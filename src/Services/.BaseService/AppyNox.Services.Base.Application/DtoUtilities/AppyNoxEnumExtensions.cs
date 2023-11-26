using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace AppyNox.Services.Base.Application.DtoUtilities
{
    public static class AppyNoxEnumExtensions
    {
        #region [ Public Methods ]

        public static Enum GetEnumValueFromDisplayName(Type enumType, string displayName)
        {
            foreach (var field in enumType.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field, typeof(DisplayAttribute)) as DisplayAttribute;
                if (attribute != null && attribute.Name == displayName)
                {
                    return (Enum)Enum.Parse(enumType, field.Name);
                }
            }
            throw new ArgumentException($"No enum value found for displayName {displayName} in {enumType}");
        }

        public static TEnum GetEnumValueFromDisplayName<TEnum>(string displayName) where TEnum : Enum
        {
            Type enumType = typeof(TEnum);

            foreach (var field in enumType.GetFields())
            {
                if (Attribute.GetCustomAttribute(field, typeof(DisplayAttribute)) is DisplayAttribute attribute && attribute.Name == displayName)
                {
                    return (TEnum)Enum.Parse(enumType, field.Name);
                }
            }

            throw new ArgumentException($"No enum value found for displayName {displayName} in {enumType}");
        }

        public static bool TryGetEnumValueFromDisplayName<TEnum>(string displayName, out TEnum? result) where TEnum : Enum
        {
            try
            {
                result = GetEnumValueFromDisplayName<TEnum>(displayName);
                return true;
            }
            catch (ArgumentException)
            {
                result = default;
                return false;
            }
        }

        public static string GetDisplayName(Enum value)
        {
            var attribute = value.GetAttributeOfType<DisplayAttribute>();

            return attribute?.Name ?? throw new ArgumentException($"DisplayName not found for enum {value}");
        }

        #endregion

        #region [ Private Methods ]

        private static T GetAttributeOfType<T>(this Enum enumValue) where T : Attribute
        {
            var type = enumValue.GetType();
            var memInfo = type.GetMember(enumValue.ToString()).First();
            var attributes = memInfo.GetCustomAttributes<T>(false);
            return attributes.FirstOrDefault() ?? throw new ArgumentException($"GetAttributeOfType error for {enumValue}");
        }

        #endregion
    }
}