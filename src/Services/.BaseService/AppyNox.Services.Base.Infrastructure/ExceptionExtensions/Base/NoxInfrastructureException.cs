using AppyNox.Services.Base.Application.Interfaces.Exceptions;
using AppyNox.Services.Base.Core.Enums;
using AppyNox.Services.Base.Core.ExceptionExtensions.Base;

namespace AppyNox.Services.Base.Infrastructure.ExceptionExtensions.Base
{
    #region [ NoxInfrastructureException Code ]

    internal enum NoxInfrastructureExceptionCode
    {
        DevelopmentError = 500,

        CommitError = 1000,

        WrongIdError = 1001,

        MultipleDataFetchingError = 1002,

        DataFetchingError = 1003,

        AddingDataError = 1004,

        UpdatingDataError = 1005,

        DeletingDataError = 1006,

        ProjectionError = 1007,

        SqlInjectionError = 1008
    }

    #endregion

    /// <summary>
    /// Represents exceptions specific to the infrastructure layer of the application.
    /// </summary>
    public class NoxInfrastructureException : NoxException, INoxInfrastructureException
    {
        #region [ Public Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="NoxInfrastructureException"/> class with a specific error message.
        /// HTTP status code is set to 500 (Internal Server Error).
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="exceptionCode">The code of the exception.</param>
        /// <param name="service">The service of the exception, representing the service where the exception is thrown.</param>
        public NoxInfrastructureException(string message, int exceptionCode, string service = "Base")
        : base(ExceptionThrownLayer.Infrastructure, service, message, exceptionCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoxInfrastructureException"/> class with a specific error message and status code.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="statusCode">The HTTP status code associated with the exception.</param>
        /// <param name="exceptionCode">The code of the exception.</param>
        /// <param name="service">The service of the exception, representing the service where the exception is thrown.</param>
        public NoxInfrastructureException(string message, int exceptionCode, int statusCode, string service = "Base")
        : base(ExceptionThrownLayer.Infrastructure, service, message, exceptionCode, statusCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoxInfrastructureException"/> class with an inner exception and an optional message.
        /// Http status code is set to 500 (Internal Server Error).
        /// </summary>
        /// <param name="ex">The inner exception.</param>
        /// <param name="exceptionCode">The code of the exception.</param>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="service">The service of the exception, representing the service where the exception is thrown.</param>
        public NoxInfrastructureException(Exception ex, int exceptionCode, string message = "Unexpected error", string service = "Base")
            : base(ExceptionThrownLayer.Infrastructure, service, message, exceptionCode, ex)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoxInfrastructureException"/> class with an inner exception and an optional message with StatusCode.
        /// </summary>
        /// <param name="ex">The inner exception.</param>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="statusCode">The HTTP status code associated with the exception.</param>
        /// <param name="exceptionCode">The code of the exception.</param>
        /// <param name="service">The service of the exception, representing the service where the exception is thrown.</param>
        public NoxInfrastructureException(Exception ex, string message, int exceptionCode, int statusCode, string service = "Base")
            : base(ExceptionThrownLayer.Infrastructure, service, message, exceptionCode, statusCode, ex)
        {
        }

        #endregion
    }
}