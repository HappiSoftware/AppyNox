using AppyNox.Services.Base.API.ExceptionExtensions.Base;
using System.Text.Json;

namespace AppyNox.Services.Base.API.Wrappers.Helpers
{
    public static class NoxResponseUnwrapper
    {
        #region [ Public Methods ]

        public static T UnwrapData<T>(string jsonResponse, JsonSerializerOptions? jsonSerializerOptions = null)
        {
            var dataElement = ExtractDataElement(jsonResponse, jsonSerializerOptions);
            return JsonSerializer.Deserialize<T>(dataElement.GetRawText(), jsonSerializerOptions)
                   ?? throw new NoxApiException("Unwrapped Object was null", (int)NoxApiExceptionCode.DevelopmentException);
        }

        public static (Guid, T) UnwrapDataWithId<T>(string jsonResponse, JsonSerializerOptions? jsonSerializerOptions = null)
        {
            var dataElement = ExtractDataElement(jsonResponse, jsonSerializerOptions);
            var id = dataElement.GetProperty("id").GetGuid();
            var createdObject = JsonSerializer.Deserialize<T>(dataElement.GetProperty("createdObject").GetRawText(), jsonSerializerOptions)
                               ?? throw new NoxApiException("Created Object was null", (int)NoxApiExceptionCode.DevelopmentException);

            return (id, createdObject);
        }

        #endregion

        #region [ Private Methods ]

        private static JsonElement ExtractDataElement(string jsonResponse, JsonSerializerOptions? jsonSerializerOptions = null)
        {
            var apiResponse = JsonSerializer.Deserialize<NoxApiResponse>(jsonResponse, jsonSerializerOptions);
            if (apiResponse?.Result is JsonElement resultElement &&
                resultElement.TryGetProperty("data", out JsonElement dataElement))
            {
                return dataElement;
            }

            throw new NoxApiException("Response does not contain expected data.", (int)NoxApiExceptionCode.DevelopmentException);
        }

        #endregion
    }
}