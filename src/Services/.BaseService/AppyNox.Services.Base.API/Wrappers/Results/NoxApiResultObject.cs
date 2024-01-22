namespace AppyNox.Services.Base.API.Wrappers.Results
{
    public class NoxApiResultObject(object? data, object? error)
    {
        #region [ Properties ]

        public object? Data { get; set; } = data;

        public object? Error { get; set; } = error;

        #endregion
    }
}