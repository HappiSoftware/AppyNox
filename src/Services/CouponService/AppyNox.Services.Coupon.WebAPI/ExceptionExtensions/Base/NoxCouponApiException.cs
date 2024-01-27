using AppyNox.Services.Base.API.ExceptionExtensions.Base;

namespace AppyNox.Services.Coupon.WebAPI.ExceptionExtensions.Base
{
    internal class NoxCouponApiException : NoxApiException
    {
        #region [ Fields ]

        private const string _service = "Coupon";

        #endregion

        #region [ Public Constructors ]

        public NoxCouponApiException(string message, int exceptionCode) : base(message, exceptionCode, _service)
        {
        }

        public NoxCouponApiException(string message, int statusCode, int exceptionCode)
            : base(message, statusCode, exceptionCode, _service)
        {
        }

        public NoxCouponApiException(Exception ex, int exceptionCode, string message = "Unexpected error")
            : base(ex, exceptionCode, message, _service)
        {
        }

        public NoxCouponApiException(Exception ex, string message, int statusCode, int exceptionCode)
            : base(ex, message, statusCode, exceptionCode, _service)
        {
        }

        #endregion
    }
}