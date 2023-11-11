using System.ComponentModel;

namespace AppyNox.Services.Authentication.Application.DtoUtilities
{
    internal static class EnumExtensions
    {
        #region [ Public Methods ]

        public static Enum GetEnumValueFromDescription(Type enumType, string description)
        {
            foreach (var field in enumType.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null && attribute.Description == description)
                {
                    return (Enum)Enum.Parse(enumType, field.Name);
                }
            }
            throw new ArgumentException($"No enum value found for description {description} in {enumType}");
        }

        #endregion
    }
}