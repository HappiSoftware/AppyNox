using AppyNox.Services.Base.Application.ExceptionExtensions.Base;
using AppyNox.Services.Base.Application.Interfaces.Exceptions;

namespace AppyNox.Services.Sso.Application.ExceptionExtensions
{
    #region [ NoxSsoApplicationException Code]

    internal enum NoxSsoApplicationExceptionCode
    {
        CreateUserCommandError = 1000,

        DeleteUserCommandError = 1001,
    }

    #endregion

    internal class NoxSsoApplicationException : NoxApplicationException, INoxApplicationException
    {
        #region [ Fields ]

        private const string _service = "Sso";

        #endregion

        #region [ Public Constructors ]

        public NoxSsoApplicationException(string message, int exceptionCode)
            : base(message, exceptionCode, _service) { }

        public NoxSsoApplicationException(string message, int exceptionCode, int statusCode)
            : base(message, exceptionCode, statusCode, _service) { }

        public NoxSsoApplicationException(
            Exception ex,
            int exceptionCode,
            string message = "Unexpected error"
        )
            : base(ex, exceptionCode, message, _service) { }

        public NoxSsoApplicationException(
            Exception ex,
            string message,
            int exceptionCode,
            int statusCode
        )
            : base(ex, message, exceptionCode, statusCode, _service) { }

        #endregion
    }
}
