using AppyNox.Services.Base.Application.Interfaces.Exceptions;
using AppyNox.Services.Base.Core.Enums;
using AppyNox.Services.Base.Infrastructure.ExceptionExtensions.Base;

namespace AppyNox.Services.Sso.Infrastructure.ExceptionExtensions.Base
{
    #region [ NoxSsoApiException Code]

    internal enum NoxSsoInfrastructureExceptionCode
    {
        UserCreationSagaCorrelationId = 1000,

        UserCreationSagaLicenseIdNull = 1001,

        UserCreationSagaCompanyIdNull = 1002,

        DeleteUserConsumerError = 1003,

        CreateUserConsumerError = 1004,
    }

    #endregion

    internal class NoxSsoInfrastructureException
        : NoxInfrastructureException,
            INoxInfrastructureException
    {
        #region [ Fields ]

        private const string _service = "Sso";

        #endregion

        #region [ Public Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="NoxSsoInfrastructureException"/> class with an inner exception and an optional message.
        /// <para>Http status code is set to 500 (Internal Server Error).</para>
        /// <para>See <see cref="NoxInfrastructureException"/> for more information.</para>
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="exceptionCode">The code of the exception.</param>
        public NoxSsoInfrastructureException(string message, int exceptionCode)
            : base(message, exceptionCode, _service) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoxSsoInfrastructureException"/> class with a specific error message and status code.
        /// <para><see cref="NoxInfrastructureException"/> for more information.</para>
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="exceptionCode">The code of the exception.</param>
        /// <param name="statusCode">The HTTP status code associated with the exception.</param>
        public NoxSsoInfrastructureException(string message, int exceptionCode, int statusCode)
            : base(message, exceptionCode, statusCode, _service) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoxSsoInfrastructureException"/> class with an inner exception and an optional message.
        /// <para>Http status code is set to 500 (Internal Server Error).</para>
        /// <para><see cref="NoxInfrastructureException"/> for more information.</para>
        /// </summary>
        /// <param name="ex">The inner exception.</param>
        /// <param name="exceptionCode">The code of the exception.</param>
        /// <param name="message">The message that describes the error.</param>
        public NoxSsoInfrastructureException(
            Exception ex,
            int exceptionCode,
            string message = "Unexpected error"
        )
            : base(ex, exceptionCode, message, _service) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoxSsoInfrastructureException"/> class with an inner exception and an optional message with StatusCode.
        /// </summary>
        /// <param name="ex">The inner exception.</param>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="exceptionCode">The code of the exception.</param>
        /// <param name="statusCode">The HTTP status code associated with the exception.</param>
        public NoxSsoInfrastructureException(
            Exception ex,
            string message,
            int exceptionCode,
            int statusCode
        )
            : base(ex, message, exceptionCode, statusCode, _service) { }

        #endregion
    }
}
