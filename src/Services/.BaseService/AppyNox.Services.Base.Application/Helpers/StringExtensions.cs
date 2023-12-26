namespace AppyNox.Services.Base.Application.Helpers;

public static class StringExtensions
{
    #region [ Public Methods ]

    public static string MinifyLogData(this string data)
    {
        return data.Replace("\\r", "").Replace("\\n", "").Replace("\\t", "").Replace("\\\"", "\"");
    }

    public static bool IsNullOrEmpty(this string? data)
    {
        return string.IsNullOrEmpty(data);
    }

    #endregion
}