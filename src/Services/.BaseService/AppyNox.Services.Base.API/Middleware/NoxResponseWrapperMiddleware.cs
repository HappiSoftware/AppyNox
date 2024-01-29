using AppyNox.Services.Base.API.ExceptionExtensions.Interfaces;
using AppyNox.Services.Base.API.Localization;
using AppyNox.Services.Base.API.Middleware.Options;
using AppyNox.Services.Base.API.Wrappers;
using AppyNox.Services.Base.API.Wrappers.Results;
using AppyNox.Services.Base.Application.ExceptionExtensions;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Core.AsyncLocals;
using AppyNox.Services.Base.Core.ExceptionExtensions.Base;
using AppyNox.Services.Base.Core.Extensions;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace AppyNox.Services.Base.API.Middleware
{
    public class NoxResponseWrapperMiddleware(RequestDelegate next,
        INoxApiLogger logger,
        NoxResponseWrapperOptions options)
    {
        #region [ Fields ]

        private readonly RequestDelegate _next = next;

        private readonly INoxApiLogger _logger = logger;

        private readonly JsonSerializerOptions _jsonSerializeOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private readonly NoxResponseWrapperOptions _options = options;

        #endregion

        #region [ Public Methods ]

        public async Task Invoke(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;

            try
            {
                using var newBodyStream = new MemoryStream();
                context.Response.Body = newBodyStream;

                await _next(context);

                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var bodyAsText = await new StreamReader(newBodyStream).ReadToEndAsync();
                context.Response.Body.Seek(0, SeekOrigin.Begin);

                if (context.Response.StatusCode >= 200 && context.Response.StatusCode < 300)
                {
                    var (isNoxResponse, noxApiResponse) = TryGetNoxApiResponse(bodyAsText);

                    if (isNoxResponse)
                    {
                        if (string.IsNullOrEmpty(noxApiResponse?.Message))
                        {
                            noxApiResponse!.Message = NoxApiResourceService.RequestSuccessful.Format(context.Request.Method);
                        }
                        await WriteResponseAsync(context, originalBodyStream, noxApiResponse);
                    }
                    else
                    {
                        object? result = DeserializeJson(bodyAsText, _jsonSerializeOptions);

                        string message = NoxApiResourceService.RequestSuccessful.Format(context.Request.Method);
                        NoxApiResponse wrappedResponse = new(result!, message, _options.ApiVersion, false, context.Response.StatusCode);
                        await WriteResponseAsync(context, originalBodyStream, wrappedResponse);
                    }
                }
                else
                {
                    string apiError = WrapUnsuccessfulError(context.Response.StatusCode);
                    string message = NoxApiResourceService.RequestUnsuccessful.Format(context.Request.Method);
                    NoxApiResponse wrappedResponse = new(apiError, message, _options.ApiVersion, true, context.Response.StatusCode);
                    await WriteResponseAsync(context, originalBodyStream, wrappedResponse);
                }
            }
            catch (NoxException noxException) when (noxException is INoxAuthenticationException)
            {
                await HandleKnownExceptionAsync(context, noxException, originalBodyStream);
            }
            catch (NoxException noxException)
            {
                _logger.LogError(noxException, NoxApiResourceService.NoxExceptionThrown);
                await HandleKnownExceptionAsync(context, noxException, originalBodyStream);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, NoxApiResourceService.UnknownExceptionThrown);
                await HandleUnknownExceptionAsync(context, exception, originalBodyStream);
            }
        }

        #endregion

        #region [ Private Methods ]

        private static object? DeserializeJson(string json, JsonSerializerOptions options)
        {
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }

            // Check if the JSON is an object/array or a simple value
            if (json.StartsWith("{") || json.StartsWith("["))
            {
                return JsonSerializer.Deserialize<object>(json, options);
            }
            else
            {
                return json.Trim('"');
            }
        }

        private static string WrapUnsuccessfulError(int statusCode)
        {
            return statusCode switch
            {
                StatusCodes.Status400BadRequest => NoxApiResourceService.BadRequestWrapper,
                StatusCodes.Status401Unauthorized => NoxApiResourceService.UnauthorizedWrapper,
                StatusCodes.Status404NotFound => NoxApiResourceService.NotFoundWrapper,
                StatusCodes.Status405MethodNotAllowed => NoxApiResourceService.MethodNotAllowedWrapper,
                StatusCodes.Status415UnsupportedMediaType => NoxApiResourceService.UnsupportedMediaTypeWrapper,
                _ => NoxApiResourceService.UnknownErrorWrapper
            };
        }

        private (bool, NoxApiResponse?) TryGetNoxApiResponse(string responseBody)
        {
            try
            {
                var response = JsonSerializer.Deserialize<NoxApiResponse>(responseBody, _jsonSerializeOptions);
                return (response != null && response.Result != null, response);
            }
            catch
            {
                // If deserialization fails, it's not a NoxApiResponse
                return (false, null);
            }
        }

        private async Task HandleKnownExceptionAsync(HttpContext context, NoxException exception, Stream originalBodyStream)
        {
            var correlationId = exception.CorrelationId;
            object errorResponse = exception is FluentValidationException fluentException
                ? new NoxApiValidationExceptionWrapObject(exception, correlationId, fluentException.ValidationResult.Errors.AsEnumerable())
                : new NoxApiExceptionWrapObject(exception, correlationId);

            var wrappedResponse = new NoxApiResponse(errorResponse, NoxApiResourceService.NoxExceptionThrown, _options.ApiVersion, true, exception.StatusCode);
            await WriteResponseAsync(context, originalBodyStream, wrappedResponse);
        }

        private async Task HandleUnknownExceptionAsync(HttpContext context, Exception exception, Stream originalBodyStream)
        {
            var errorResponse = new { Message = exception.Message, exception.StackTrace };
            var wrappedResponse = new NoxApiResponse(errorResponse, NoxApiResourceService.UnknownExceptionThrown, _options.ApiVersion, true, StatusCodes.Status500InternalServerError);
            await WriteResponseAsync(context, originalBodyStream, wrappedResponse);
        }

        private async Task WriteResponseAsync(HttpContext context, Stream originalBodyStream, NoxApiResponse response)
        {
            response.Result = !response.HasError ? new NoxApiResultObject(response.Result, null) : new NoxApiResultObject(null, response.Result);
            var responseContent = JsonSerializer.Serialize(response, _jsonSerializeOptions);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = response.Code ?? StatusCodes.Status200OK;
            context.Response.Body = originalBodyStream;
            await context.Response.WriteAsync(responseContent);
        }

        #endregion
    }
}