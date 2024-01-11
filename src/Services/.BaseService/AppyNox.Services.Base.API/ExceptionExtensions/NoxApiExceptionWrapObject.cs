using AppyNox.Services.Base.Domain.ExceptionExtensions.Base;
using AutoWrapper;

namespace AppyNox.Services.Base.API.ExceptionExtensions
{
    /// <summary>
    /// Represents a wrapper object for API exceptions, used for standardizing error responses.
    /// </summary>
    public class NoxApiExceptionWrapObject(NoxException error, string correlationId)
    {
        #region [ Properties ]

        [AutoWrapperPropertyMap(Prop.ResponseException)]
        public object Error { get; set; } = new NoxError(error, correlationId);

        #endregion

        #region [ Classes ]

        protected class NoxError(NoxException error, string correlationId)
        {
            #region [ Properties ]

            public int Code { get; set; } = error.StatusCode;

            public string Title { get; set; } = $"{error.Service} {error.Layer}";

            public string Message { get; set; } = error.Message;

            public string CorrelationId { get; set; } = correlationId;

            public Exception? InnerException { get; set; } = error.InnerException;

            #endregion
        }

        #endregion
    }
}