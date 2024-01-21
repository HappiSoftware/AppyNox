using Newtonsoft.Json;

namespace AppyNox.Services.Base.API.Wrappers
{
    public class NoxApiResponse(object result, string message = "", string version = "1.0", bool hasError = false, int? code = null)
    {
        #region [ Properties ]

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; } = message;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Version { get; set; } = version;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool HasError { get; set; } = hasError;

        [JsonIgnore]
        public int? Code { get; set; } = code;

        public object Result { get; set; } = result;

        #endregion
    }
}