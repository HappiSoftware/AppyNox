using AppyNox.Services.Base.Application.Interfaces.Exceptions;
using AppyNox.Services.Base.Core.Enums;
using AppyNox.Services.Base.Core.ExceptionExtensions.Base;

namespace AppyNox.Services.Base.Application.ExceptionExtensions.Base
{
    #region [ NoxApplicationException Code]

    internal enum NoxApplicationExceptionCode
    {
        AccessTypeEmpty = 1000,

        AccessTypeError = 1001,

        DtoDetailLevelNotFoundForDisplay = 1002,

        DtoDetailLevelNotFound = 1003,

        CommonDtoLevelIsNotFound = 1004,

        FluentValidationError = 1005,

        ValidatorNotFound = 1006,

        GenericCreateCommandError = 1007,

        GenericGetAllQueryError = 1008,

        GenericGetByIdQueryError = 1009,

        GenericUpdateCommandError = 1010,

        GenericDeleteCommandError = 1011,

        MapEntitiesError = 1012,

        MapEntityError = 1013,

        UnsupportedLevelAttribute = 1014,
    }

    #endregion

    /// <summary>
    /// Represents exceptions specific to the application layer of the application.
    /// </summary>
    public class NoxApplicationException : NoxException, INoxApplicationException
    {
        #region [ Public Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="NoxApplicationException"/> class with a specific error message.
        /// Http status code is set to 500.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="exceptionCode">The code of the exception.</param>
        public NoxApplicationException(string message, int exceptionCode, string service = "Base")
            : base(ExceptionThrownLayer.Application, service, message, exceptionCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoxApplicationException"/> class with a specific error message and status code.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="exceptionCode">The code of the exception.</param>
        /// <param name="statusCode">The HTTP status code associated with the exception.</param>
        public NoxApplicationException(string message, int exceptionCode, int statusCode, string service = "Base")
            : base(ExceptionThrownLayer.Application, service, message, exceptionCode, statusCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoxApplicationException"/> class with an inner exception and an optional message.
        /// Http status code is set to 500.
        /// </summary>
        /// <param name="ex">The inner exception.</param>
        /// <param name="exceptionCode">The code of the exception.</param>
        /// <param name="message">The message that describes the error. Defaults to "Unexpected error" if not provided.</param>
        public NoxApplicationException(Exception ex, int exceptionCode, string message = "Unexpected error", string service = "Base")
            : base(ExceptionThrownLayer.Application, service, message, exceptionCode, ex)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoxApplicationException"/> class with an inner exception and an optional message with StatusCode.
        /// </summary>
        /// <param name="ex">The inner exception.</param>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="exceptionCode">The code of the exception.</param>
        /// <param name="statusCode">The HTTP status code associated with the exception.</param>
        /// <param name="service">The service of the exception, representing the service where the exception is thrown.</param>
        public NoxApplicationException(Exception ex, string message, int exceptionCode, int statusCode, string service = "Base")
            : base(ExceptionThrownLayer.Application, service, message, exceptionCode, statusCode, ex)
        {
        }

        #endregion
    }
}