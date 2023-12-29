using AppyNox.Services.Base.Application.ExceptionExtensions.Base;
using FluentValidation.Results;
using System.Net;

namespace AppyNox.Services.Base.Application.ExceptionExtensions
{
    /// <summary>
    /// Exception thrown when FluentValidation detects validation errors in a DTO.
    /// </summary>
    public class FluentValidationException(Type dtoType, ValidationResult validationResult)
        : NoxApplicationException($"Request responded with one or more validation errors for '{dtoType}'", (int)HttpStatusCode.BadRequest)
    {
        #region [ Properties ]

        /// <summary>
        /// Gets the validation result containing details about the validation errors.
        /// </summary>
        public ValidationResult ValidationResult { get; } = validationResult;

        #endregion
    }
}