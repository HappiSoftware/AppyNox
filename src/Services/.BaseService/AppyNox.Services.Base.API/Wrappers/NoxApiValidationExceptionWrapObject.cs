using AppyNox.Services.Base.Core.Exceptions.Base;
using FluentValidation.Results;
using System.Text.Json.Serialization;

namespace AppyNox.Services.Base.API.Wrappers;

/// <summary>
/// Represents a wrapper object for API validation exceptions, extending the standard error response with validation errors.
/// </summary>
internal class NoxApiValidationExceptionWrapObject(NoxException error, Guid correlationId, IEnumerable<ValidationFailure> validationErrors)
    : NoxApiExceptionWrapObject(error, correlationId)
{
    #region [ Properties ]

    [JsonPropertyOrder(6)]
    public IEnumerable<ValidationFailure> ValidationErrors { get; set; } = validationErrors;

    #endregion
}