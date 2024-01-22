using AppyNox.Services.Base.API.ExceptionExtensions.Base;
using System.Net;

namespace AppyNox.Services.Base.API.ExceptionExtensions
{
    public class NoxTokenExpiredException : NoxApiException
    {
        #region [ Public Constructors ]

        public NoxTokenExpiredException()
            : base("Token has expired.", (int)HttpStatusCode.Unauthorized)
        {
        }

        #endregion
    }
}