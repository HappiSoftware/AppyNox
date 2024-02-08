using AppyNox.Services.Base.Core.ExceptionExtensions.Base;

namespace AppyNox.Services.Base.API.Wrappers;

/// <summary>
/// Represents a wrapper object for API exceptions, used for standardizing error responses.
/// </summary>
internal class NoxApiExceptionWrapObject(NoxException error, Guid correlationId)
{
    #region [ Properties ]

    public int ExceptionCode { get; set; } = error.ExceptionCode;

    public string Title { get; set; } = $"{error.Service} {error.Layer}";

    public string Message { get; set; } = error.Message;

    public Guid CorrelationId { get; set; } = correlationId;

    public NoxSimpleExceptionData? InnerException { get; set; } = error.InnerException != null ? new NoxSimpleExceptionData(error.InnerException) : null;

    #endregion
}

internal class NoxSimpleExceptionData(Exception exception)
{
    #region [ Properties ]

    public string Message { get; set; } = exception.Message;

    public string? StackTrace { get; set; } = exception.StackTrace;

    #endregion
}