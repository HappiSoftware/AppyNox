using AppyNox.Services.Base.Application.ExceptionExtensions.Base;
using AppyNox.Services.Base.Application.Localization;
using AppyNox.Services.Base.Core.Extensions;
using FluentValidation.Results;
using System.Net;

namespace AppyNox.Services.Base.Application.ExceptionExtensions
{
    /// <summary>
    /// Exception thrown when FluentValidation detects validation errors in a DTO.
    /// </summary>
    public class FluentValidationException(Type dtoType, ValidationResult validationResult)
        : NoxApplicationException(
            message: NoxApplicationResourceService.FluentValidationFailed.Format(dtoType),
            exceptionCode: (int)NoxApplicationExceptionCode.FluentValidationError,
            statusCode: (int)HttpStatusCode.UnprocessableContent)
    {
        #region [ Properties ]

        /// <summary>
        /// Gets the validation result containing details about the validation errors.
        /// </summary>
        public ValidationResult ValidationResult { get; } = validationResult;

        #endregion
    }
}