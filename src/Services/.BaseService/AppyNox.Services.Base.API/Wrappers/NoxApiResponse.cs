using System.Text.Json.Serialization;

namespace AppyNox.Services.Base.API.Wrappers
{
    public class NoxApiResponse(object result, string message = "", string version = "1.0", bool hasError = false, int? code = null)
    {
        #region [ Properties ]

        [JsonPropertyName("message")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Message { get; set; } = message;

        [JsonPropertyName("version")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Version { get; set; } = version;

        [JsonPropertyName("hasError")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool HasError { get; set; } = hasError;

        [JsonIgnore]
        public int? Code { get; } = code;

        [JsonPropertyName("result")]
        public object Result { get; set; } = result;

        #endregion
    }
}