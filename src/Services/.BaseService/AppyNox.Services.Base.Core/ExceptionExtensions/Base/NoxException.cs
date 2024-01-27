using AppyNox.Services.Base.Core.AsyncLocals;
using AppyNox.Services.Base.Core.Extensions;
using System.Net;
using System.Text.Json.Serialization;

namespace AppyNox.Services.Base.Core.ExceptionExtensions.Base
{
    /// <summary>
    /// Represents a base class for custom exceptions in the application.
    /// <para>It is not suggested to use this exception directly in microservices. Please check the documentation for more information.</para>
    /// </summary>
    public abstract class NoxException : Exception, INoxException
    {
        #region [ Fields ]

        private readonly string _service = "Base";

        private readonly string _layer = "Core";

        private readonly int _statusCode;

        private readonly int _exceptionCode;

        private readonly Guid _correlationId = CorrelationContext.CorrelationId;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets the HTTP status code associated with the exception.
        /// </summary>
        [JsonIgnore]
        public int StatusCode => _statusCode;

        /// <summary>
        /// Gets the layer of the exception, typically representing the layer where the exception is thrown. Ex: Application or Infrastructure
        /// </summary>
        public string Layer => _layer;

        /// <summary>
        /// Gets the Service of the exception, typically representing the service where the exception is thrown. Ex: Base or Authentication
        /// </summary>
        public string Service => _service;

        /// <summary>
        /// Gets the correlation id of the request
        /// </summary>
        public Guid CorrelationId => _correlationId;

        /// <summary>
        /// Gets the exception code of the exception
        /// </summary>
        public int ExceptionCode => _exceptionCode;

        #endregion

        #region [ Public Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="NoxException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        protected NoxException(string message)
            : base(message)
        {
            _statusCode = (int)HttpStatusCode.InternalServerError;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoxException"/> class with a layer, a specified error message, and default status code 500.
        /// </summary>
        /// <param name="layer">The layer of the exception, representing the layer where the exception is thrown.</param>
        /// <param name="service">The service of the exception, representing the service where the exception is thrown.</param>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="exceptionCode">The code of the associated exception.</param>
        protected NoxException(Enum layer, string service, string message, int exceptionCode)
            : base(message)
        {
            _layer = layer.GetDisplayName();
            _statusCode = (int)HttpStatusCode.InternalServerError;
            _service = service;
            _exceptionCode = exceptionCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoxException"/> class with a layer, a specified error message, and status code.
        /// </summary>
        /// <param name="layer">The layer of the exception, representing the layer where the exception is thrown.</param>
        /// <param name="service">The service of the exception, representing the service where the exception is thrown.</param>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="statusCode">The HTTP status code associated with the exception.</param>
        /// <param name="exceptionCode">The code of the associated exception.</param>
        protected NoxException(Enum layer, string service, string message, int exceptionCode, int statusCode)
            : base(message)
        {
            _layer = layer.GetDisplayName();
            _statusCode = statusCode;
            _service = service;
            _exceptionCode = exceptionCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoxException"/> class with a layer, a specified error message, and a reference to the inner exception that is the cause of this exception.
        /// status code will be 500.
        /// </summary>
        /// <param name="layer">The layer of the exception, representing the layer where the exception is thrown.</param>
        /// <param name="service">The service of the exception, representing the service where the exception is thrown.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="exceptionCode">The code of the associated exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        protected NoxException(Enum layer, string service, string message, int exceptionCode, Exception innerException)
            : base(message, innerException)
        {
            _layer = layer.GetDisplayName();
            _statusCode = (int)HttpStatusCode.InternalServerError;
            _service = service;
            _exceptionCode = exceptionCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoxException"/> class with a layer, a specified error message, and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="layer">The layer of the exception, representing the layer where the exception is thrown.</param>
        /// <param name="service">The service of the exception, representing the service where the exception is thrown.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="exceptionCode">The code of the associated exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        protected NoxException(Enum layer, string service, string message, int exceptionCode, int statusCode, Exception innerException)
            : base(message, innerException)
        {
            _layer = layer.GetDisplayName();
            _statusCode = statusCode;
            _service = service;
            _exceptionCode = exceptionCode;
        }

        #endregion
    }
}