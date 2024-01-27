using AppyNox.Services.Base.Application.Interfaces.Exceptions;
using AppyNox.Services.Base.Infrastructure.ExceptionExtensions.Base;
using System.Net;

namespace AppyNox.Services.License.Infrastructure.ExceptionExtensions
{
    #region [ NoxLicenseInfrastructureException Code]

    internal enum NoxLicenseInfrastructureExceptionCode
    {
        LicenseServiceInfrastructureError = 999,
    }

    #endregion

    internal class NoxLicenseInfrastructureException : NoxInfrastructureException, INoxInfrastructureException
    {
        #region [ Fields ]

        private const string _service = "License";

        #endregion

        #region [ Public Constructors ]

        public NoxLicenseInfrastructureException(string message,
                                                 int exceptionCode = (int)NoxLicenseInfrastructureExceptionCode.LicenseServiceInfrastructureError)
            : base(message, exceptionCode, _service)
        {
        }

        public NoxLicenseInfrastructureException(string message,
                                                 int exceptionCode = (int)NoxLicenseInfrastructureExceptionCode.LicenseServiceInfrastructureError,
                                                 int statusCode = (int)HttpStatusCode.InternalServerError)
            : base(message, exceptionCode, statusCode, _service)
        {
        }

        public NoxLicenseInfrastructureException(Exception ex,
                                                 int exceptionCode = (int)NoxLicenseInfrastructureExceptionCode.LicenseServiceInfrastructureError,
                                                 string message = "Unexpected error")
            : base(ex, exceptionCode, message, _service)
        {
        }

        public NoxLicenseInfrastructureException(Exception ex,
                                                 string message,
                                                 int exceptionCode = (int)NoxLicenseInfrastructureExceptionCode.LicenseServiceInfrastructureError,
                                                 int statusCode = (int)HttpStatusCode.InternalServerError)
           : base(ex, message, statusCode, exceptionCode, _service)
        {
        }

        #endregion
    }
}