using AppyNox.Services.Base.Core.ExceptionExtensions.Base;

namespace AppyNox.Services.Base.API.Wrappers
{
    /// <summary>
    /// Represents a wrapper object for API exceptions, used for standardizing error responses.
    /// </summary>
    internal class NoxApiExceptionWrapObject(NoxException error, Guid correlationId)
    {
        #region [ Properties ]

        public int Code { get; set; } = error.StatusCode;

        public string Title { get; set; } = $"{error.Service} {error.Layer}";

        public string Message { get; set; } = error.Message;

        public Guid CorrelationId { get; set; } = correlationId;

        public Exception? InnerException { get; set; } = error.InnerException;

        #endregion
    }
}