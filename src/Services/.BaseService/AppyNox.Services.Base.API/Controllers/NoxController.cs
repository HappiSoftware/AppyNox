using AppyNox.Services.Base.API.ExceptionExtensions.Base;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AppyNox.Services.Base.API.Controllers
{
    public abstract class NoxController : Controller
    {
        #region [ Protected Methods ]

        protected string GetUserIdFromJwtToken()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new NoxApiException("Jwt Token does not have NameIdentifier", 401);
        }

        #endregion
    }
}