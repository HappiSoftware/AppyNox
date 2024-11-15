namespace AppyNox.Services.Base.API.Wrappers.Results;

public class NoxApiResultObject<TData>(TData? data, object? error)
{
    #region [ Properties ]

    public TData? Data { get; set; } = data;

    public object? Error { get; set; } = error;

    #endregion
}