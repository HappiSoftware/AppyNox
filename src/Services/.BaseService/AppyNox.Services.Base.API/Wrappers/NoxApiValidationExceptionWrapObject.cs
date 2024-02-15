using AppyNox.Services.Base.Core.ExceptionExtensions.Base;
using FluentValidation.Results;

namespace AppyNox.Services.Base.API.Wrappers;

/// <summary>
/// Represents a wrapper object for API validation exceptions, extending the standard error response with validation errors.
/// </summary>
internal class NoxApiValidationExceptionWrapObject(NoxException error, Guid correlationId, IEnumerable<ValidationFailure> validationErrors)
    : NoxApiExceptionWrapObject(error, correlationId)
{
    #region [ Properties ]

    public IEnumerable<ValidationFailure> ValidationErrors { get; set; } = validationErrors;

    #endregion
}