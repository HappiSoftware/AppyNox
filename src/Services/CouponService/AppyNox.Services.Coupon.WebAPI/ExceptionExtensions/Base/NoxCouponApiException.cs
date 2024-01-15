using AppyNox.Services.Base.API.ExceptionExtensions.Base;

namespace AppyNox.Services.Coupon.WebAPI.ExceptionExtensions.Base
{
    internal class NoxCouponApiException : NoxApiException
    {
        #region [ Fields ]

        private const string _service = "Coupon";

        #endregion

        #region [ Public Constructors ]

        public NoxCouponApiException(string message) : base(message, _service)
        {
        }

        public NoxCouponApiException(string message, int statusCode)
            : base(message, statusCode, _service)
        {
        }

        public NoxCouponApiException(Exception ex, string message = "Unexpected error")
            : base(ex, message, _service)
        {
        }

        public NoxCouponApiException(Exception ex, string message, int statusCode)
            : base(ex, message, statusCode, _service)
        {
        }

        #endregion
    }
}