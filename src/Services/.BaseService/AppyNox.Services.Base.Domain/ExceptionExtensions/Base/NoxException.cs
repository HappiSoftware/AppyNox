using AppyNox.Services.Base.Domain.ExceptionExtensions.Enums;
using AppyNox.Services.Base.Domain.Helpers;
using System.Net;

namespace AppyNox.Services.Base.Domain.ExceptionExtensions.Base
{
    /// <summary>
    /// Represents a base class for custom exceptions in the application.
    /// </summary>
    public abstract class NoxException : Exception
    {
        #region [ Fields ]

        private readonly string _title = ExceptionThrownLayer.DomainBase.ToString();

        private readonly int _statusCode;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets the HTTP status code associated with the exception.
        /// </summary>
        public int StatusCode => _statusCode;

        /// <summary>
        /// Gets the title of the exception, typically representing the layer where the exception is thrown.
        /// </summary>
        public string Title => _title;

        #endregion

        #region [ Public Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="NoxException"/> class with default status code.
        /// </summary>
        protected NoxException()
            : base()
        {
            _statusCode = (int)HttpStatusCode.InternalServerError;
        }

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
        /// Initializes a new instance of the <see cref="NoxException"/> class with a specified error message and status code.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="statusCode">The HTTP status code associated with the exception.</param>
        protected NoxException(string message, int statusCode)
            : base(message)
        {
            _statusCode = statusCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoxException"/> class with a title, a specified error message, and default status code 500.
        /// </summary>
        /// <param name="title">The title of the exception, representing the layer where the exception is thrown.</param>
        /// <param name="message">The message that describes the error.</param>
        protected NoxException(Enum title, string message)
            : base(message)
        {
            _title = title.GetDisplayName();
            _statusCode = (int)HttpStatusCode.InternalServerError;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoxException"/> class with a title, a specified error message, and status code.
        /// </summary>
        /// <param name="title">The title of the exception, representing the layer where the exception is thrown.</param>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="statusCode">The HTTP status code associated with the exception.</param>
        protected NoxException(Enum title, string message, int statusCode)
            : base(message)
        {
            _title = title.GetDisplayName();
            _statusCode = statusCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoxException"/> class with a title, a specified error message, and a reference to the inner exception that is the cause of this exception.
        /// status code will be 500.
        /// </summary>
        /// <param name="title">The title of the exception, representing the layer where the exception is thrown.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        protected NoxException(Enum title, string message, Exception innerException)
            : base(message, innerException)
        {
            _title = title.GetDisplayName();
            _statusCode = (int)HttpStatusCode.InternalServerError;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoxException"/> class with a title, a specified error message, and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="title">The title of the exception, representing the layer where the exception is thrown.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        protected NoxException(Enum title, string message, int statusCode, Exception innerException)
            : base(message, innerException)
        {
            _title = title.GetDisplayName();
            _statusCode = statusCode;
        }

        #endregion
    }
}