using AppyNox.Services.Base.Domain.ExceptionExtensions.Base;
using AppyNox.Services.Base.Domain.ExceptionExtensions.Enums;

namespace AppyNox.Services.Authentication.WebAPI.ExceptionExtensions.Base;

public class AuthenticationBaseException(string message, int statusCode)
        : NoxException(ExceptionThrownLayer.ApiBase, message, statusCode)
{
}
