using AppyNox.Services.Base.API.ExceptionExtensions.Base;
using AppyNox.Services.Base.Domain.ExceptionExtensions.Base;
using AppyNox.Services.Base.Domain.ExceptionExtensions.Enums;

namespace AppyNox.Services.Authentication.WebAPI.ExceptionExtensions.Base;

/// <summary>
/// Exception type for handling authentication-related errors in Api layer.
/// </summary>
public class AuthenticationServiceException(string message, int statusCode, string service = "Authentication")
        : NoxApiException(message, statusCode, service)
{
}