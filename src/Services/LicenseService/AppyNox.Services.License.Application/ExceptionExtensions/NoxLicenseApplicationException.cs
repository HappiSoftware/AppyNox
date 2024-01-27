using AppyNox.Services.Base.Application.ExceptionExtensions.Base;
using AppyNox.Services.Base.Application.Interfaces.Exceptions;
using System.Net;

namespace AppyNox.Services.License.Application.ExceptionExtensions
{
    #region [ NoxLicenseApplicationException Code]

    internal enum NoxLicenseApplicationExceptionCode
    {
        LicenseServiceApplicationError = 999,

        AssignKeyCommandError = 1000,
    }

    #endregion

    internal class NoxLicenseApplicationException : NoxApplicationException, INoxApplicationException
    {
        #region [ Fields ]

        private const string _service = "License";

        #endregion

        #region [ Public Constructors ]

        public NoxLicenseApplicationException(string message,
                                              int exceptionCode = (int)NoxLicenseApplicationExceptionCode.LicenseServiceApplicationError)
            : base(message, exceptionCode, _service)
        {
        }

        public NoxLicenseApplicationException(string message,
                                              int exceptionCode = (int)NoxLicenseApplicationExceptionCode.LicenseServiceApplicationError,
                                              int statusCode = (int)HttpStatusCode.InternalServerError)
            : base(message, exceptionCode, statusCode, _service)
        {
        }

        public NoxLicenseApplicationException(Exception ex,
                                              int exceptionCode = (int)NoxLicenseApplicationExceptionCode.LicenseServiceApplicationError,
                                              string message = "Unexpected error")
            : base(ex, exceptionCode, message, _service)
        {
        }

        public NoxLicenseApplicationException(Exception ex,
                                              string message,
                                              int exceptionCode = (int)NoxLicenseApplicationExceptionCode.LicenseServiceApplicationError,
                                              int statusCode = (int)HttpStatusCode.InternalServerError)
            : base(ex, message, statusCode, exceptionCode, _service)
        {
        }

        #endregion
    }
}