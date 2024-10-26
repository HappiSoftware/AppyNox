# Exception Handling

<br>

## NoxException

`NoxException` is a base Exception class used in AppyNox. In addition to the `System.Exception` class, `NoxException`
contains '`StatusCode`' and '`Title`' properties. Exceptions thrown as `NoxException` will be caught by the `ExceptionHandlingMiddleware`
in `AppyNox.Services.Base.API`, and the mentioned properties will be used in `NoxApiResponse` before being thrown in `AppyNox.Services.Base.API.Middleware.NoxResponseWrapperMiddleware`.

The '`StatusCode`' and '`Title`' properties are readonly and can only be set from Constructors. It's important to note that `NoxException` is an **abstract** class
and cannot be instantiated. So, how do we throw errors? Let's explore:

<br>

## Layer-Based Exceptions

For each layer in AppyNox's Onion Architecture, we create new classes that inherit from `NoxException`. For example, `NoxApplicationException`.
Directly using `NoxApplicationException` is not conventional. This exception is used in BaseService. In the microservices created, it is suggested to create a new exception `Nox{ServiceName}ApplicationException`

<br>

`NoxException:`

<details>
  <summary>Click to expand <i>NoxException</i></summary>

```cs
using AppyNox.Services.Base.Core.AsyncLocals;
using AppyNox.Services.Base.Core.Common;
using AppyNox.Services.Base.Core.Extensions;
using System.Net;
using System.Text.Json.Serialization;

namespace AppyNox.Services.Base.Core.Exceptions.Base;

/// <summary>
/// Represents a base class for custom exceptions in the application.
/// <para>It is not suggested to use this exception directly in microservices. Please check the documentation for more information.</para>
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="NoxException"/> class with a layer, a specified error message, and a reference to the inner exception that is the cause of this exception.
/// </remarks>
/// <param name="product">The product of the exception, representing the service where the exception is thrown.</param>
/// <param name="service">The service of the exception, representing the service where the exception is thrown.</param>
/// <param name="layer">The layer of the exception, representing the layer where the exception is thrown.</param>
/// <param name="message">The error message that explains the reason for the exception.</param>
/// <param name="exceptionCode">The code of the associated exception.</param>
/// <param name="innerException">The exception that is the cause of the current exception.</param>
public abstract class NoxException(
    Enum product,
    string service,
    Enum layer,
    string? message = null,
    int? exceptionCode = null,
    int? statusCode = null,
    Exception? innerException = null) : Exception(message ?? NoxExceptionStrings.EmptyMessage, innerException), INoxException
{
    #region [ Fields ]

    private readonly string _product = product.GetDisplayName();

    private readonly string _service = service;

    private readonly string _layer = layer.GetDisplayName();

    private readonly int _exceptionCode = exceptionCode ?? 999;

    private readonly int _statusCode = statusCode ?? (int)HttpStatusCode.InternalServerError;

    private readonly Guid _correlationId = NoxContext.CorrelationId;

    private readonly string _innerExceptionMessage = innerException?.Message ?? NoxExceptionStrings.EmptyMessage;

    #endregion

    #region [ Properties ]

    /// <summary>
    /// Gets the Product of the exception, typically representing the service where the exception is thrown. Ex: Nox or Fleet
    /// </summary>
    public string Product => _product;

    /// <summary>
    /// Gets the Service of the exception, typically representing the service where the exception is thrown. Ex: Sso or Coupon
    /// </summary>
    public string Service => _service;

    /// <summary>
    /// Gets the layer of the exception, typically representing the layer where the exception is thrown. Ex: Application or Infrastructure
    /// </summary>
    public string Layer => _layer;

    /// <summary>
    /// Gets the exception code of the exception
    /// </summary>
    public int ExceptionCode => _exceptionCode;

    /// <summary>
    /// Gets the HTTP status code associated with the exception.
    /// </summary>
    [JsonIgnore]
    public int StatusCode => _statusCode;

    /// <summary>
    /// Gets the correlation id of the request
    /// </summary>
    public Guid CorrelationId => _correlationId;

    /// <summary>
    /// Gets the inner exception message
    /// </summary>
    public string InnerExceptionMessage => _innerExceptionMessage;

    #endregion
}
```

</details>

<br>

`NoxApplicationException:`

<details>
  <summary>Click to expand <i>NoxApplicationException</i></summary>

```cs
using AppyNox.Services.Base.Application.Interfaces.Exceptions;
using AppyNox.Services.Base.Core.Common;
using AppyNox.Services.Base.Core.Enums;
using AppyNox.Services.Base.Core.Exceptions.Base;

namespace AppyNox.Services.Base.Application.Exceptions.Base;

#region [ NoxApplicationException Code ]

internal enum NoxApplicationExceptionCode
{
    AccessTypeEmpty = 1000,

    AccessTypeError = 1001,

    DtoDetailLevelNotFoundForDisplay = 1002,

    DtoDetailLevelNotFound = 1003,

    IUpdateDtoNullId = 1004,

    FluentValidationError = 1005,

    ValidatorNotFound = 1006,

    GenericCreateCommandError = 1007,

    GenericGetAllQueryError = 1008,

    GenericGetByIdQueryError = 1009,

    GenericUpdateCommandError = 1010,

    GenericDeleteCommandError = 1011,

    MapEntitiesError = 1012,

    MapEntityError = 1013,

    UnsupportedLevelAttribute = 1014,

    MismatchedIdInUpdate = 1015,

    DtoTypesForEntityMethodError = 1016
}

#endregion

/// <summary>
/// Provides a constructor for derived exception classes in the application layer, ensuring all
/// application-related exceptions are standardized.
/// </summary>
/// <remarks>
/// This constructor is part of an abstract class and is intended to be invoked by constructors of derived classes.
/// It sets initial values for exception properties, with defaults provided for optional parameters.
/// This standardization helps in maintaining consistency across all exceptions thrown within the application layer.
/// <para>
/// Example of using this constructor in a derived class:
/// <code>
/// internal class NoxCouponApplicationException(
///     string? message = null,
///     int? exceptionCode = null,
///     int? statusCode = null,
///     Exception? innerException = null)
///     : NoxApplicationExceptionBase(
///         ExceptionProduct.AppyNox,       // DisplayName of the enum: "Nox"
///         NoxCouponCommonStrings.Service, // "Coupon"
///         message,
///         exceptionCode,
///         statusCode,
///         innerException)
/// {
/// }
/// </code>
/// </para>
/// </remarks>
/// <param name="product">The product associated with the exception, representing the high-level category of the service
/// or component where the exception originated.</param>
///
/// <param name="service">The specific service within the product that the exception pertains to.</param>
///
/// <param name="message">The message that describes the error. This message is optional; a default message may be used
/// if not provided (A default error message was not provided.).</param>
///
/// <param name="exceptionCode">The custom code identifying the type of exception. This is optional and a default may be
/// defined if not provided (999).</param>
///
/// <param name="statusCode">The HTTP status code associated with the exception. This is optional; a default status code
/// may be used if not provided (500).</param>
///
/// <param name="innerException">The inner exception that is the cause of this exception. This is optional and can be null
/// if there is no inner exception.</param>
public abstract class NoxApplicationExceptionBase(
    Enum product,
    string service,
    string? message = null,
    int? exceptionCode = null,
    int? statusCode = null,
    Exception? innerException = null)
    : NoxException(product,
        service,
        ExceptionThrownLayer.Application,
        message,
        exceptionCode,
        statusCode,
        innerException), INoxApplicationException
{
}

internal class NoxApplicationException(
    string? message = null,
    int? exceptionCode = null,
    int? statusCode = null,
    Exception? innerException = null)
    : NoxApplicationExceptionBase(
        ExceptionProduct.AppyNox,
        NoxExceptionStrings.Base,
        message,
        exceptionCode,
        statusCode,
        innerException)
{
}
```

</details>

<br>

`AccessTypeNotFoundException:`

<details>
    <summary>Click to expand <i>AccessTypeNotFoundException</i></summary>

```cs
using AppyNox.Services.Base.Application.Exceptions.Base;
using AppyNox.Services.Base.Application.Localization;
using AppyNox.Services.Base.Core.Common;
using AppyNox.Services.Base.Core.Enums;
using AppyNox.Services.Base.Core.Extensions;
using System.Net;

namespace AppyNox.Services.Base.Application.Exceptions;

/// <summary>
/// Exception thrown when an access type mapping for an entity is not found.
/// </summary>
internal class AccessTypeNotFoundException : NoxApplicationException
{
    #region [ Internal Constructors ]

    /// <summary>
    /// Initializes a new instance of the <see cref="AccessTypeNotFoundException"/> class for a specific entity type.
    /// Http status code is set to 500.
    /// </summary>
    /// <param name="entity">The entity type for which the access level mapping is not found.</param>
    internal AccessTypeNotFoundException(Type entity)
        : base(
            message: NoxApplicationResourceService.EntityHasNoAccessLevel.Format(entity.FullName ?? entity.Name),
            exceptionCode: (int)NoxApplicationExceptionCode.AccessTypeEmpty)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AccessTypeNotFoundException"/> class for a specific entity type and access type.
    /// Http status code is set to 400.
    /// </summary>
    /// <param name="entity">The entity type for which the access level mapping is not found.</param>
    /// <param name="accessType">The specific access type that is not found.</param>
    internal AccessTypeNotFoundException(Type entity, string accessType)
        : base(
            message: NoxApplicationResourceService.EntityHasNoAccessLevelForType.Format(entity.FullName ?? entity.Name, accessType),
            exceptionCode: (int)NoxApplicationExceptionCode.AccessTypeError,
            statusCode: (int)HttpStatusCode.BadRequest)
    {
    }

    #endregion
}
```

</details>

<br>
<br>
<br>
<br>
<br>

## Exception Codes

Each ```NoxException``` has it's unique code. NoxException Codes start from **'1000'**. Keep in mind that each layer's exception codes start from '1000', which means you can get '1000' for different exceptions, however you should be aware of the **layer**.

<br>
<br>

### Nox Application Exception Codes

- **1000-AccessTypeEmpty** : Indicates that the related Entity type has no access levels. It is a server error. Please contact reach customer service with the related Correlation Id.

- **1001-AccessTypeError** : Indicates that the related Entity does not have the requested AccessLevel. Please send the request again with correct parameters.

- **1002-DtoDetailLevelNotFoundForDisplay** : Indicates that the requested DetailLevel is not found. Please send the request again with correct parameters.

- **1003-DtoDetailLevelNotFound** : Indicates that the requested DetailLevel is not found. Unlike 1002, it is a server error. Please reach customer service with the related Correlation Id. 

- **1004-IUpdateDtoNullId** : Indicates that user requested a PUT request however Id property was missing in the request body

- **1005-FluentValidationError** : Indicates that validation rules failed for the request. Please send the request again with correct values.

- **1006-ValidatorNotFound** : Indicates that no validator found for the request body dto. It is a server error. Please reach customer service with the related Correlation Id.

- **1007-GenericCreateCommandError** : Indicates that there was an unexpected error while adding a new entity using GenericService. It is a server error. Please reach customer service with the related Correlation Id.

- **1008-GenericGetAllQueryError** : Indicates that there was an unexpected error while fetching the data from GenericService. It is a server error. Please reach customer service with the related Correlation Id.

- **1009-GenericGetByIdQueryError** : Indicates that there was an unexpected error while fetching the data from GenericService. It is a server error. Please reach customer service with the related Correlation Id.

- **1010-GenericUpdateCommandError** : Indicates that there was an unexpected error while updating an entity using GenericService. It is a server error. Please reach customer service with the related Correlation Id.

- **1011-GenericDeleteCommandError** : Indicates that there was an unexpected error while deleting an entity using GenericService. It is a server error. Please reach customer service with the related Correlation Id.

- **1012-MapEntitiesError** : Indicates that there fetching data was successful however there was a problem on mapping entities to dto. It is a server error. Please reach customer service with the related Correlation Id.

- **1013-MapEntityError** : Indicates that there fetching data was successful however there was a problem on mapping singnle entity to dto. It is a server error. Please reach customer service with the related Correlation Id.

- **1014-UnsupportedLevelAttribute** : Indicates that there is a problem for in the DtoMappingRegistry. Please reach customer service with the related Correlation Id.

- **1015-MismatchedIdInUpdate** : Indicates that the Id in the request body and the Id in the Query Parameter of the request was not matching.

- **1016-DtoTypesForEntityMethodError** : Indicates that Dto Mapping could not be found. It is a system eror. It is possible that the Entity is not included in the DtoMappingRegistry. Please contact reach customer service with the related Correlation Id.

<br>
<br>

### Nox Infrastructure Exception Codes

- **500-DevelopmentError** : Errors indicating there was an error during startup of the project.

- **1000-CommitError** : Indicates that there was a database error. It is a server error. Please reach customer service with the related Correlation Id.

- **1001-WrongIdError** : Indicates that the requested Id is not existing in the database. Please correct the id and send the request again.

- **1002-MultipleDataFetchingError** : Indicates that there was an unexpected error while fetching data from the database for multiple entities. It is a server error. Please reach customer service with the related Correlation Id.

- **1003-DataFetchingError** : Indicates that there was an unexpected error while fetching data from the database for a single entity. It is a server error. Please reach customer service with the related Correlation Id.

- **1004-AddingDataError** : Indicates that there was an unexpected error while adding data to the database. It is a server error. Please reach customer service with the related Correlation Id.

- **1005-UpdatingDataError** : Indicates that there was an unexpected error while updating data on the database. It is a server error. Please reach customer service with the related Correlation Id.

- **1006-DeletingDataError** : Indicates that there was an unexpected error while deleting data on the database. It is a server error. Please reach customer service with the related Correlation Id.

- **1007-ProjectionError** : Indicates that there was an unexpected error while creating projection for the entity. It is a server error. Please reach customer service with the related Correlation Id.

- **1008-QueryParameterError** : Indicates that QueryParameters of the request was in a wrong format.

- **1009-SqlInjectionError** : Indicates that the request contains malicious keywords.

- **1010-InvalidEncryptionSettingError** : Indicates that the service is using Encryption however there was a problem with the settings.

- **1011-AuthenticationInvalidToken** : Indicates that the provided token was not recognized. Login again and use the provided token.

- **1012-AuthenticationNullToken** : Indicates that the token is not provided. Include the token retrieved from the Sso and send the request again.

- **1013-ExpiredToken** : Indicates that the token is not expired. Login again to refresh the token.

- **1014-AuthorizationFailed** : Indicates that the user can not take this action because the user does not have the authorization.

- **1015-AuthorizationInvalidToken** : Indicates that the provided token was not recognized. Login again and use the provided token.

<br>
<br>

### Nox Api Exception Codes

- **500-DevelopmentError** : NoxUnwrapper exceptions. Thrown in integration testing only.

- **1000-CorrelationIdError** : Indicates that the request caught by NoxApi does not have a CorrelationId. Correlation Id's are required for request consistency. Normally Gateway is handling this operation automatically. If you see this error, please reach customer service immediately.

- **1001-SwaggerGenerationException** : Indicates that there was a problem in Swagger generation.

<br>
<br>

### Nox Sso Application Exception Codes

- **1000-CreateUserCommandError** :  Indicates that there was an unexpected error while creating a new user using Commands. It is a server error. Please reach customer service with the related Correlation Id.

- **1001-DeleteUserCommandError** :  Indicates that there was an unexpected error while deleting the user using Commands. It is a server error. Please reach customer service with the related Correlation Id.

<br>
<br>

### Nox Sso Infrastructure Exception Codes
- **1000-UserCreationSagaCorrelationId** : Indicates that the correlation ID was null during the User Creation Saga.

- **1001-UserCreationSagaLicenseIdNull** : Indicates that that the License ID is null during the User Creation Saga process.

- **1002-UserCreationSagaCompanyIdNull** : Indicates that that the Company ID is null during the User Creation Saga process.

- **1003-DeleteUserConsumerError** : Indicates that an unexpected error occurred while deleting a user through the consumer service.

- **1004-CreateUserConsumerError** : Indicates that an unexpected error occurred while deleting a user through the consumer service.

- **1005-UserCreationSagaLicenseIdOrCompanyIdNullError** : Indicates that either the License ID or Company ID is null during the User Creation Saga.

- **1005-ExpiredToken** : This error means the provided token has expired. Please re-authenticate with a valid token.

- **1005-AuthorizationFailed** : Indicates that authorization failed due to insufficient permissions or an invalid token.

- **1005-AuthorizationInvalidToken** : The authorization token is invalid. Ensure you are using a valid token.

- **1005-AuthenticationInvalidToken** : This error means the authentication token is invalid. Please authenticate again.

- **1005-InvalidAudience** : The provided token has an audience mismatch. Please make sure your audience when logging in.

- **1005-RefreshToken** : Indicates that there was an unexpected error while saving the refresh token. Authenticate again.

- **1005-WrongCredentials** : Indicates that the provided credentials are incorrect. Verify your credentials and try again.

- **1005-RefreshTokenNotFound** : The refresh token could not be located. Please re-authenticate or make sure you are using the correct Refresh Token.

<br>
<br>

### Nox Sso Api Exception Codes

- **1000-RefreshTokenInvalid** : Indicates that the provided refresh token was not matching. Make sure you are using the correct refresh token.


<br>
<br>
<br>
<br>
<br>

# Advanced

For the ones who wants to learn where these exceptions are caught and wrapped, check the codes below:

<details>
  <summary>Click to expand <i>NoxResponseWrapperMiddleware</i></summary>

```cs
using AppyNox.Services.Base.API.Localization;
using AppyNox.Services.Base.API.Middleware.Options;
using AppyNox.Services.Base.API.Wrappers;
using AppyNox.Services.Base.API.Wrappers.Results;
using AppyNox.Services.Base.Application.Exceptions;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Core.Exceptions.Base;
using AppyNox.Services.Base.Core.Extensions;
using AppyNox.Services.Base.Infrastructure.Exceptions.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace AppyNox.Services.Base.API.Middleware;

public class NoxResponseWrapperMiddleware(RequestDelegate next,
        INoxApiLogger<NoxResponseWrapperMiddleware> logger,
        NoxResponseWrapperOptions options)
{
    #region [ Fields ]

    private readonly RequestDelegate _next = next;

    private readonly INoxApiLogger<NoxResponseWrapperMiddleware> _logger = logger;

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
            using MemoryStream newBodyStream = new();
            context.Response.Body = newBodyStream;

            await _next(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            string bodyAsText = await new StreamReader(newBodyStream).ReadToEndAsync();
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
                    object? resultBody = DeserializeJson(bodyAsText);
                    NoxApiResultObject result = new(resultBody, null);
                    string message = NoxApiResourceService.RequestSuccessful.Format(context.Request.Method);
                    NoxApiResponse wrappedResponse = new(result, message, _options.ApiVersion, false, context.Response.StatusCode);
                    await WriteResponseAsync(context, originalBodyStream, wrappedResponse);
                }
            }
            else
            {
                string apiError = WrapUnsuccessfulError(context.Response.StatusCode);
                string message = NoxApiResourceService.RequestUnsuccessful.Format(context.Request.Method);
                NoxApiResultObject errorResponse = new(null, apiError);
                NoxApiResponse wrappedResponse = new(errorResponse, message, _options.ApiVersion, true, context.Response.StatusCode);
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

    private static string WrapUnsuccessfulError(int statusCode)
    {
        return statusCode switch
        {
            StatusCodes.Status400BadRequest => NoxApiResourceService.BadRequestWrapper,
            StatusCodes.Status401Unauthorized => NoxApiResourceService.UnauthorizedWrapper,
            StatusCodes.Status403Forbidden => NoxApiResourceService.ForbiddenAccess,
            StatusCodes.Status404NotFound => NoxApiResourceService.NotFoundWrapper,
            StatusCodes.Status405MethodNotAllowed => NoxApiResourceService.MethodNotAllowedWrapper,
            StatusCodes.Status415UnsupportedMediaType => NoxApiResourceService.UnsupportedMediaTypeWrapper,
            _ => NoxApiResourceService.UnknownErrorWrapper
        };
    }

    private object? DeserializeJson(string json)
    {
        if (string.IsNullOrEmpty(json))
        {
            return null;
        }

        // Check if the JSON is an object/array or a simple value
        if (json.StartsWith("{") || json.StartsWith("["))
        {
            return JsonSerializer.Deserialize<object>(json, _jsonSerializeOptions);
        }
        else
        {
            return json.Trim('"');
        }
    }

    private (bool, NoxApiResponse?) TryGetNoxApiResponse(string responseBody)
    {
        try
        {
            NoxApiResponsePOCO? response = JsonSerializer.Deserialize<NoxApiResponsePOCO>(responseBody, _jsonSerializeOptions);
            return (response != null && response.Result != null, new NoxApiResponse(response!.Result!, response.Message, response.Version, response.HasError, response.Code));
        }
        catch
        {
            // If deserialization fails, it's not a NoxApiResponse
            return (false, null);
        }
    }

    private async Task HandleKnownExceptionAsync(HttpContext context, NoxException exception, Stream originalBodyStream)
    {
        Guid correlationId = exception.CorrelationId;
        object errorResponseBody = exception is NoxFluentValidationException fluentException
            ? new NoxApiValidationExceptionWrapObject(exception, correlationId, fluentException.ValidationResult.Errors.AsEnumerable())
            : new NoxApiExceptionWrapObject(exception, correlationId);

        NoxApiResultObject errorResponse = new(null, errorResponseBody);
        NoxApiResponse wrappedResponse = new(errorResponse, NoxApiResourceService.NoxExceptionThrown, _options.ApiVersion, true, exception.StatusCode);
        await WriteResponseAsync(context, originalBodyStream, wrappedResponse);
    }

    private async Task HandleUnknownExceptionAsync(HttpContext context, Exception exception, Stream originalBodyStream)
    {
        var errorResponseBody = new { exception.Message, exception.StackTrace };
        NoxApiResultObject errorResponse = new(null, errorResponseBody);
        NoxApiResponse wrappedResponse = new(errorResponse, NoxApiResourceService.UnknownExceptionThrown, _options.ApiVersion, true, StatusCodes.Status500InternalServerError);
        await WriteResponseAsync(context, originalBodyStream, wrappedResponse);
    }

    private async Task WriteResponseAsync(HttpContext context, Stream originalBodyStream, NoxApiResponse response)
    {
        context.Response.StatusCode = response.Code;

        // No content should be written to the response body for a 204 response
        if (context.Response.StatusCode != StatusCodes.Status204NoContent)
        {
            response.Code = 0; // will be ignored in response body
            string responseContent = JsonSerializer.Serialize(response, _jsonSerializeOptions);

            context.Response.ContentType = "application/json";
            context.Response.Body = originalBodyStream;
            await context.Response.WriteAsync(responseContent);
        }
        else
        {
            // If it's a 204 No Content response, make sure the body is empty.
            context.Response.ContentLength = 0;
            await originalBodyStream.FlushAsync();
        }
    }

    #endregion
}
```

</details>