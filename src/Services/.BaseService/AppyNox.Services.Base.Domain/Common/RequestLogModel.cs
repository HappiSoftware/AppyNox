namespace AppyNox.Services.Base.Domain.Common;

public class RequestLogModel
{
    #region Public Constructors

    public RequestLogModel(string method, string path, string? queryString, string? body)
    {
        Method = method;
        Path = path;
        QueryString = queryString;
        Body = body;
    }

    #endregion

    #region Properties

    public string Method { get; set; }

    public string Path { get; set; }

    public string? QueryString { get; set; }

    public string? Body { get; set; }

    #endregion

    // others .. if needed
}