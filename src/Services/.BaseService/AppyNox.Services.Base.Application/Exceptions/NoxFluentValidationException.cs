using AppyNox.Services.Base.Application.Exceptions.Base;
using AppyNox.Services.Base.Application.Localization;
using AppyNox.Services.Base.Core.Common;
using AppyNox.Services.Base.Core.Enums;
using AppyNox.Services.Base.Core.Extensions;
using FluentValidation.Results;
using System.Net;

namespace AppyNox.Services.Base.Application.Exceptions;

/// <summary>
/// Exception thrown when FluentValidation detects validation errors in a DTO.
/// </summary>
public class NoxFluentValidationException(Type dtoType, ValidationResult validationResult)
    : NoxApplicationExceptionBase(
        ExceptionProduct.AppyNox,
        NoxExceptionStrings.Base,
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