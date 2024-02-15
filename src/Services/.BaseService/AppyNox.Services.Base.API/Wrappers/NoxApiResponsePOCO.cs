using AppyNox.Services.Base.API.Wrappers.Results;

namespace AppyNox.Services.Base.API.Wrappers;

internal class NoxApiResponsePOCO
{
    #region [ Properties ]

    public string Message { get; set; } = string.Empty;

    public string Version { get; set; } = string.Empty;

    public bool HasError { get; set; }

    public int Code { get; set; }

    public NoxApiResultObject Result { get; set; } = null!;

    #endregion
}