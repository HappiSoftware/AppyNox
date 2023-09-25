using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Authentication.Application.DtoUtilities
{
    internal static class EnumExtensions
    {
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
    }
}
