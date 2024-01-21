namespace AppyNox.Services.Base.API.Wrappers.Results
{
    internal class NoxApiSuccessResult(object data)
    {
        #region [ Properties ]

        public object Data { get; set; } = data;

        public object? Error { get; set; } = null;

        #endregion
    }
}