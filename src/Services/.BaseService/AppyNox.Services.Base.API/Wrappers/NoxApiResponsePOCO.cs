using AppyNox.Services.Base.API.Wrappers.Results;

namespace AppyNox.Services.Base.API.Wrappers;

internal class NoxApiResponsePOCO<TData>
{
    #region [ Properties ]

    public string Message { get; set; } = string.Empty;

    public string Version { get; set; } = string.Empty;

    public bool HasError { get; set; }

    public int Code { get; set; }

    public NoxApiResultObject<TData> Result { get; set; } = null!;

    #endregion
}