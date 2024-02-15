namespace AppyNox.Services.Base.IntegrationTests.Wrapper;

public class NoxApiExceptionWrapObjectPOCO
{
    #region [ Properties ]

    public string Title { get; set; } = string.Empty;

    public int ExceptionCode { get; set; }

    public string Message { get; set; } = string.Empty;

    public Guid CorrelationId { get; set; }

    #endregion

    #region Properties

    public NoxSimpleExceptionDataPOCO? InnerException { get; set; }

    #endregion
}

public class NoxSimpleExceptionDataPOCO
{
    #region [ Properties ]

    public string Message { get; set; } = string.Empty;

    public string? StackTrace { get; set; }

    #endregion
}