namespace AppyNox.Services.Base.API.Wrappers.Results
{
    internal class NoxApiErrorResult(object data)
    {
        #region [ Properties ]

        public object? Data { get; set; } = null;

        public object? Error { get; set; } = data;

        #endregion
    }
}