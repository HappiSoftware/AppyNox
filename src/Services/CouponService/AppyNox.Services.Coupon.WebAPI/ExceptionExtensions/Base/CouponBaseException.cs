using AppyNox.Services.Base.Domain.ExceptionExtensions.Base;
using AppyNox.Services.Base.Domain.ExceptionExtensions.Enums;

namespace AppyNox.Services.Coupon.WebAPI.ExceptionExtensions.Base;

public class CouponBaseException(string message, int statusCode)
        : NoxException(ExceptionThrownLayer.ApiBase, message, statusCode)
{
}
