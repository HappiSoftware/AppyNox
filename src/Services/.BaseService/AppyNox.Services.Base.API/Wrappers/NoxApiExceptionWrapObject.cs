﻿using AppyNox.Services.Base.Core.Exceptions.Base;
using System.Text.Json.Serialization;

namespace AppyNox.Services.Base.API.Wrappers;

/// <summary>
/// Represents a wrapper object for API exceptions, used for standardizing error responses.
/// </summary>
internal class NoxApiExceptionWrapObject(NoxException error, Guid correlationId)
{
    #region [ Properties ]

    [JsonPropertyOrder(1)]
    public string Title { get; set; } = $"{error.Product} {error.Service} {error.Layer}";

    [JsonPropertyOrder(2)]
    public int ExceptionCode { get; set; } = error.ExceptionCode;

    [JsonPropertyOrder(3)]
    public string Message { get; set; } = error.Message;

    [JsonPropertyOrder(4)]
    public Guid CorrelationId { get; set; } = correlationId;

    [JsonPropertyOrder(5)]
    public NoxSimpleExceptionData? InnerException { get; set; } = error.InnerException != null ? new NoxSimpleExceptionData(error.InnerException, error.InnerExceptionMessage) : null;

    #endregion
}

internal class NoxSimpleExceptionData(Exception exception, string innerExceptionMessage)
{
    #region [ Properties ]

    public string Message { get; set; } = innerExceptionMessage;

    public string? StackTrace { get; set; } = exception.StackTrace;

    #endregion
}