using AppyNox.Services.Base.Core.Common;
using AppyNox.Services.Base.Infrastructure.Exceptions.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AppyNox.Services.Coupon.Infrastructure.Authentication;

public class CouponTokenManager(CouponTokenConfiguration couponTokenConfiguration) : ICouponTokenManager
{
    private readonly JwtSecurityTokenHandler _tokenHandler = new();
    private readonly CouponTokenConfiguration _couponTokenConfiguration = couponTokenConfiguration;

    /// <summary>
    /// Used for testing multiple authentication/authorization scheme implementations. 
    /// This class does not have an actual role.
    /// </summary>
    /// <returns>string token</returns>
    public string CreateToken()
    {
        List<Claim> claims = [];

        claims.Add(new Claim(ClaimTypes.Name, "CouponUser"));
        claims.Add(new Claim(ClaimTypes.NameIdentifier, "1"));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(5),
            Issuer = _couponTokenConfiguration.Issuer,
            Audience = _couponTokenConfiguration.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_couponTokenConfiguration.GetSecretKeyBytes()), SecurityAlgorithms.HmacSha256Signature),
            IssuedAt = DateTime.UtcNow,
        };

        var token = _tokenHandler.CreateToken(tokenDescriptor);
        return _tokenHandler.WriteToken(token);
    }

    public async Task<bool> VerifyToken(string token)
    {
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(_couponTokenConfiguration.GetSecretKeyBytes()),
            ValidateLifetime = true,
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidAudience = _couponTokenConfiguration.Audience,
            ValidIssuer = _couponTokenConfiguration.Issuer,
            ClockSkew = TimeSpan.Zero
        };

        try
        {
            _tokenHandler.ValidateToken(token, validationParameters, out SecurityToken sToken);
            return await Task.FromResult(true);
        }
        catch (SecurityTokenExpiredException)
        {
            throw new InfrastructureException("Expired token. Dev.", 999);
        }
        catch (Exception)
        {
            throw new InfrastructureException("Invalid token. Dev.", 999);
        }
    }
}