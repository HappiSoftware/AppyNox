using AppyNox.Services.Base.Core.Exceptions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace AppyNox.Services.Base.Core.Extensions;

/// <summary>
/// Provides extension methods for enums.
/// </summary>
public static class NoxEnumExtensionsBase
{
    #region [ Public Methods ]

    /// <summary>
    /// Gets the display name attribute of an enum value.
    /// </summary>
    /// <param name="value">The enum value.</param>
    /// <returns>The display name of the enum value.</returns>
    /// <exception cref="EnumDisplayNameNotFoundException">
    /// Thrown if the display name is not found for the enum value.
    /// </exception>
    public static string GetDisplayName(this Enum value)
    {
        var attribute = value.GetAttributeOfType<DisplayAttribute>();

        return attribute?.Name ?? throw new EnumDisplayNameNotFoundException(value);
    }

    public static string GetDescription(this Enum e)
    {
        var name = e.ToString();
        var memberInfo = e.GetType().GetMember(name)[0];
        var descriptionAttributes = memberInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
        if (descriptionAttributes.Length != 0) return ((DescriptionAttribute)descriptionAttributes.First()).Description;
        return name;
    }

    #endregion

    #region [ Private Methods ]

    /// <summary>
    /// Gets an attribute of a specified type from an enum value.
    /// </summary>
    /// <typeparam name="T">The type of attribute to get.</typeparam>
    /// <param name="enumValue">The enum value.</param>
    /// <returns>The attribute of the specified type.</returns>
    /// <exception cref="EnumDisplayNameNotFoundException">
    /// Thrown if the attribute of the specified type is not found for the enum value.
    /// </exception>
    private static T GetAttributeOfType<T>(this Enum enumValue) where T : Attribute
    {
        var type = enumValue.GetType();
        var memInfo = type.GetMember(enumValue.ToString())[0];
        var attributes = memInfo.GetCustomAttributes<T>(false);
        return attributes.FirstOrDefault() ?? throw new EnumDisplayNameNotFoundException(enumValue);
    }

    #endregion
}