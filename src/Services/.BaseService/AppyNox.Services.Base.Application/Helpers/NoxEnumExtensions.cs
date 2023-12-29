﻿using AppyNox.Services.Base.Application.ExceptionExtensions;
using AppyNox.Services.Base.Domain.Helpers;
using System.ComponentModel.DataAnnotations;

namespace AppyNox.Services.Base.Application.Helpers
{
    /// <summary>
    /// Provides extension methods for working with enums in the application.
    /// </summary>
    public static class NoxEnumExtensions
    {
        #region [ Public Methods ]

        /// <summary>
        /// Retrieves an enum value from its display name.
        /// </summary>
        /// <param name="enumType">The type of the enum.</param>
        /// <param name="displayName">The display name of the enum value.</param>
        /// <returns>The corresponding enum value.</returns>
        /// <exception cref="DtoDetailLevelNotFoundException">Thrown when no matching enum value is found for the given display name.</exception>
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

        /// <summary>
        /// Retrieves the display name of an enum value.
        /// </summary>
        /// <param name="value">The enum value.</param>
        /// <returns>The display name of the enum value.</returns>
        public static string GetDisplayName(this Enum value)
        {
            return NoxEnumExtensionsBase.GetDisplayName(value);
        }

        #endregion
    }
}