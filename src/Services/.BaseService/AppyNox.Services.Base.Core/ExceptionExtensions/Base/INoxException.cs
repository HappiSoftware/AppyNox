namespace AppyNox.Services.Base.Core.ExceptionExtensions.Base
{
    public interface INoxException
    {
        #region [ Properties ]

        int StatusCode { get; }

        string Layer { get; }

        string Service { get; }

        Guid CorrelationId { get; }

        #endregion
    }
}