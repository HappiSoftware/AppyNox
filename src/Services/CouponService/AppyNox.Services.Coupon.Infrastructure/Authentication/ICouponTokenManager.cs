
namespace AppyNox.Services.Coupon.Infrastructure.Authentication
{
    public interface ICouponTokenManager
    {
        string CreateToken();
        Task<bool> VerifyToken(string token);
    }
}