using AppyNox.Services.Base.API.Wrappers.Results;
using System.Net;
using System.Text.Json.Serialization;

namespace AppyNox.Services.Base.API.Wrappers;

public class NoxApiResponse
{
    #region [ Properties ]

    [JsonPropertyName("message")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Message { get; set; }

    [JsonPropertyName("version")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Version { get; set; }

    [JsonPropertyName("hasError")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool HasError { get; set; }

    [JsonPropertyName("code")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int Code { get; internal set; }

    [JsonPropertyName("result")]
    public NoxApiResultObject Result { get; set; }

    #endregion

    #region [ Constructors ]

    public NoxApiResponse(object result, string message = "", string version = "1.0", bool hasError = false, int code = (int)HttpStatusCode.OK)
        : this(new NoxApiResultObject(result, null), message, version, hasError, code)
    {
    }

    internal NoxApiResponse(NoxApiResultObject result, string message = "", string version = "1.0", bool hasError = false, int code = (int)HttpStatusCode.OK)
    {
        Result = result;
        Message = message;
        Version = version;
        HasError = hasError;
        Code = code;
    }

    #endregion
}