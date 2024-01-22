using AppyNox.Services.Base.API.Constants;

namespace AppyNox.Services.Base.API.Middleware.Options
{
    public class NoxResponseWrapperOptions
    {
        #region [ Properties ]

        public string ApiVersion { get; set; } = NoxVersions.v1_0;

        #endregion
    }
}