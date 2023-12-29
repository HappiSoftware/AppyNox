namespace AppyNox.Services.Base.Application.Helpers;

/// <summary>
/// Provides extension methods for string manipulation, particularly for logging and validation purposes.
/// </summary>
public static class StringExtensions
{
    #region [ Public Methods ]

    /// <summary>
    /// Minifies a JSON string for logging by removing unnecessary characters like newlines and tabs.
    /// </summary>
    /// <param name="data">The JSON string to minify.</param>
    /// <returns>The minified JSON string.</returns>
    public static string MinifyLogData(this string data)
    {
        return data.Replace("\\r", "").Replace("\\n", "").Replace("\\t", "").Replace("\\\"", "\"");
    }

    /// <summary>
    /// Checks whether a string is null or empty.
    /// </summary>
    /// <param name="data">The string to check.</param>
    /// <returns>True if the string is null or empty; otherwise, false.</returns>
    public static bool IsNullOrEmpty(this string? data)
    {
        return string.IsNullOrEmpty(data);
    }

    #endregion
}