using AppyNox.Services.Base.API.ExceptionExtensions.Base;
using System.Net;

namespace AppyNox.Services.Authentication.WebAPI.ExceptionExtensions.Base
{
    #region [ NoxAuthenticationApiException Code]

    internal enum NoxSsoApiExceptionCode
    {
        SsoServiceApiError = 999,

        AuthenticationInvalidToken = 1000,

        AuthenticationNullToken = 1001,

        ExpiredToken = 1002,

        AuthorizationFailed = 1003,

        AuthorizationInvalidToken = 1004,

        InvalidAudience = 1005,

        RefreshToken = 1006,

        WrongCredentials = 1007,

        RefreshTokenNotFound = 1008,

        SignInError = 1009,

        Teapot = 1010,
    }

    #endregion

    /// <summary>
    /// Exception type for handling authentication-related errors in Api layer.
    /// </summary>
    public class NoxSsoApiException : NoxApiException
    {
        #region [ Fields ]

        private const string _service = "Authentication";

        #endregion

        #region [ Public Constructors ]

        public NoxSsoApiException(string message,
                                             int exceptionCode = (int)NoxSsoApiExceptionCode.SsoServiceApiError)
            : base(message, exceptionCode, _service)
        {
        }

        public NoxSsoApiException(string message,
                                             int exceptionCode = (int)NoxSsoApiExceptionCode.SsoServiceApiError,
                                             int statusCode = (int)HttpStatusCode.InternalServerError)
            : base(message, exceptionCode, statusCode, _service)
        {
        }

        public NoxSsoApiException(Exception ex,
                                             int exceptionCode = (int)NoxSsoApiExceptionCode.SsoServiceApiError,
                                             string message = "Unexpected error")
            : base(ex, exceptionCode, message, _service)
        {
        }

        public NoxSsoApiException(Exception ex,
                                             string message,
                                             int exceptionCode = (int)NoxSsoApiExceptionCode.SsoServiceApiError,
                                             int statusCode = (int)HttpStatusCode.InternalServerError)
            : base(ex, message, exceptionCode, statusCode, _service)
        {
        }

        #endregion
    }
}