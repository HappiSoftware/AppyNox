using AppyNox.Services.Base.Application.ExceptionExtensions.Base;
using AppyNox.Services.Base.Application.Localization;
using FluentValidation.Results;
using System.Net;
using AppyNox.Services.Base.Core.Extensions;

namespace AppyNox.Services.Base.Application.ExceptionExtensions
{
    /// <summary>
    /// Exception thrown when FluentValidation detects validation errors in a DTO.
    /// </summary>
    public class FluentValidationException(Type dtoType, ValidationResult validationResult)
        : NoxApplicationException(NoxApplicationResourceService.FluentValidationFailed.Format(dtoType), (int)HttpStatusCode.BadRequest)
    {
        #region [ Properties ]

        /// <summary>
        /// Gets the validation result containing details about the validation errors.
        /// </summary>
        public ValidationResult ValidationResult { get; } = validationResult;

        #endregion
    }
}