using AppyNox.Services.Base.API.Wrappers;
using System.Text.Json;

namespace AppyNox.Services.Base.IntegrationTests.Wrapper.Helpers;

public static class NoxResponseUnwrapper
{
    #region [ Public Methods ]

    public static async Task<NoxApiResponse> UnwrapResponse(HttpResponseMessage response, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        string jsonResponse = await response.Content.ReadAsStringAsync();
        NoxApiResponsePOCO responsePOCO = JsonSerializer.Deserialize<NoxApiResponsePOCO>(jsonResponse, jsonSerializerOptions)
            ?? throw new Exception("Unwrapper - Response was null");

        responsePOCO.Code = (int)response.StatusCode;

        if (!responsePOCO.HasError)
        {
            return new NoxApiResponse(responsePOCO);
        }

        string? errorJson = responsePOCO.Result.Error?.ToString();
        if (responsePOCO.HasError && responsePOCO.Result?.Error != null && !string.IsNullOrEmpty(errorJson))
        {
            try
            {
                var noxValidationDetails = JsonSerializer.Deserialize<NoxApiValidationExceptionWrapObjectPOCO>(errorJson, jsonSerializerOptions);
                if (noxValidationDetails?.ValidationErrors != null)
                {
                    responsePOCO.Result.Error = noxValidationDetails;
                }
                else
                {
                    var noxExceptionDetails = JsonSerializer.Deserialize<NoxApiExceptionWrapObjectPOCO>(errorJson, jsonSerializerOptions);
                    responsePOCO.Result.Error = noxExceptionDetails;
                }
            }
            catch (Exception) // string error body
            {
                responsePOCO.Result.Error = errorJson;
            }
        }
        return new NoxApiResponse(responsePOCO);
    }

    public static T UnwrapData<T>(string jsonResponse, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        var dataElement = ExtractDataElement(jsonResponse, jsonSerializerOptions);
        return JsonSerializer.Deserialize<T>(dataElement.GetRawText(), jsonSerializerOptions)
               ?? throw new Exception("Unwrapper - Unwrapped Object was null");
    }

    public static (Guid, T) UnwrapDataWithId<T>(string jsonResponse, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        var dataElement = ExtractDataElement(jsonResponse, jsonSerializerOptions);
        var id = dataElement.GetProperty("id").GetGuid();
        var createdObject = JsonSerializer.Deserialize<T>(dataElement.GetProperty("createdObject").GetRawText(), jsonSerializerOptions)
                           ?? throw new Exception("Unwrapper - Created Object was null");

        return (id, createdObject);
    }

    #endregion

    #region [ Private Methods ]

    private static JsonElement ExtractDataElement(string jsonResponse, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        var apiResponse = JsonSerializer.Deserialize<NoxApiResponsePOCO>(jsonResponse, jsonSerializerOptions);
        if (apiResponse?.Result != null &&
            apiResponse.Result.Data is JsonElement dataElement)
        {
            return dataElement;
        }

        throw new Exception("Unwrapper - Response does not contain expected data.");
    }

    #endregion
}