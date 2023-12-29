namespace AppyNox.Services.Base.Domain.Common;

/// <summary>
/// Model representing the log information of an HTTP request.
/// </summary>
public class RequestLogModel(string method, string path, string? queryString, string? body)
{
    #region [ Properties ]

    /// <summary>
    /// Gets or sets the HTTP method of the request (e.g., GET, POST).
    /// </summary>
    public string Method { get; set; } = method;

    /// <summary>
    /// Gets or sets the path of the request.
    /// </summary>
    public string Path { get; set; } = path;

    /// <summary>
    /// Gets or sets the query string of the request, if any.
    /// </summary>
    public string? QueryString { get; set; } = queryString;

    /// <summary>
    /// Gets or sets the body content of the request, if any.
    /// </summary>
    public string? Body { get; set; } = body;

    #endregion
}