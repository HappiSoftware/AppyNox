namespace AppyNox.Services.Base.Core.Exceptions.Base;

public interface INoxException
{
    #region [ Properties ]

    string Product { get; }

    string Service { get; }

    string Layer { get; }

    public int ExceptionCode { get; }

    int StatusCode { get; }

    Guid CorrelationId { get; }

    #endregion
}