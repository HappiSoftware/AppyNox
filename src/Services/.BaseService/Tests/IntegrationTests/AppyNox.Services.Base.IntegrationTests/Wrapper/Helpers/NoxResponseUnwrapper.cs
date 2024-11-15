using AppyNox.Services.Base.API.Wrappers;
using AppyNox.Services.Base.API.Wrappers.Results;
using System.Text.Json;
using static MassTransit.ValidationResultExtensions;

namespace AppyNox.Services.Base.IntegrationTests.Wrapper.Helpers;

public static class NoxResponseUnwrapper
{
    #region [ Public Methods ]

    public static async Task<NoxApiResponse<TData>> UnwrapResponse<TData>(HttpResponseMessage response, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        string jsonResponse = await response.Content.ReadAsStringAsync();
        NoxApiResponsePOCO<TData>? responsePOCO;
        try
        {
            responsePOCO = JsonSerializer.Deserialize<NoxApiResponsePOCO<TData>>(jsonResponse, jsonSerializerOptions)
            ?? throw new Exception("Could not parsed.");
        }
        catch (Exception)
        {
            NoxApiResponsePOCO<object> responsePOCO2 = JsonSerializer.Deserialize<NoxApiResponsePOCO<object>>(jsonResponse, jsonSerializerOptions)!;
            responsePOCO = new NoxApiResponsePOCO<TData>()
            {
                Message = responsePOCO2.Message,
                Code = responsePOCO2.Code,
                HasError = responsePOCO2.HasError,
                Version = responsePOCO2.Version,
                Result = new NoxApiResultObject<TData>(default, responsePOCO2.Result?.Error)
            };
        }

        responsePOCO.Code = (int)response.StatusCode;

        if (!responsePOCO.HasError)
        {
            return new NoxApiResponse<TData>(responsePOCO);
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
        return new NoxApiResponse<TData>(responsePOCO);
    }

    public static TData UnwrapData<TData>(string jsonResponse, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        var dataElement = ExtractDataElement<TData>(jsonResponse, jsonSerializerOptions);
        return JsonSerializer.Deserialize<TData>(dataElement.GetRawText(), jsonSerializerOptions)
               ?? throw new Exception("Unwrapper - Unwrapped Object was null");
    }

    #endregion

    #region [ Private Methods ]

    private static JsonElement ExtractDataElement<TData>(string jsonResponse, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        var apiResponse = JsonSerializer.Deserialize<NoxApiResponsePOCO<TData>>(jsonResponse, jsonSerializerOptions);
        if (apiResponse?.Result != null &&
            apiResponse.Result.Data is JsonElement dataElement)
        {
            return dataElement;
        }

        throw new Exception("Unwrapper - Response does not contain expected data.");
    }

    #endregion
}