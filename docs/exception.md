# Exception Handling

<br>

**NoxException**

`NoxException` is a base Exception class used in AppyNox. In addition to the `System.Exception` class, `NoxException`
contains '`StatusCode`' and '`Title`' properties. Exceptions thrown as `NoxException` will be caught by the `ExceptionHandlingMiddleware`
in `AppyNox.Services.Base.API`, and the mentioned properties will be used in `NoxApiException` before being thrown in `AutoWrapper.ApiException`.

The '`StatusCode`' and '`Title`' properties are readonly and can only be set from Constructors. It's important to note that `NoxException` is an **abstract** class
and cannot be instantiated. So, how do we throw errors? Let's explore:

<br>

**Layer-Based Exceptions**

For each layer in AppyNox's Onion Architecture, we create new classes that inherit from `NoxException`. For example, `NoxApplicationException`.
Directly using `NoxApplicationException` is not conventional. This exception is used in BaseService. In the microservices created, it is suggested to create a new exception `Nox{ServiceName}ApplicationException`

<br>

`NoxException:`

<details>
  <summary>Click to expand <i>NoxException</i></summary>

```cs
namespace AppyNox.Services.Base.Core.ExceptionExtensions.Base
{
    /// <summary>
    /// Represents a base class for custom exceptions in the application.
    /// <para>It is not suggested to use this exception directly in microservices. Please check the documentation for more information.</para>
    /// </summary>
    public abstract class NoxException : Exception, INoxException
    {
        #region [ Fields ]

        private readonly string _service = "Base";

        private readonly string _layer = "Core";

        private readonly int _statusCode;

        private readonly Guid _correlationId = CorrelationContext.CorrelationId;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets the HTTP status code associated with the exception.
        /// </summary>
        public int StatusCode => _statusCode;

        /// <summary>
        /// Gets the layer of the exception, typically representing the layer where the exception is thrown. Ex: Application or Infrastructure
        /// </summary>
        public string Layer => _layer;

        /// <summary>
        /// Gets the Service of the exception, typically representing the service where the exception is thrown. Ex: Base or Authentication
        /// </summary>
        public string Service => _service;

        /// <summary>
        /// Gets the correlation id of the request
        /// </summary>
        public Guid CorrelationId => _correlationId;

        #endregion

        #region [ Public Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="NoxException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        protected NoxException(string message)
            : base(message)
        {
            _statusCode = (int)HttpStatusCode.InternalServerError;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoxException"/> class with a layer, a specified error message, and default status code 500.
        /// </summary>
        /// <param name="layer">The layer of the exception, representing the layer where the exception is thrown.</param>
        /// <param name="service">The service of the exception, representing the service where the exception is thrown.</param>
        /// <param name="message">The message that describes the error.</param>
        protected NoxException(Enum layer, string service, string message)
            : base(message)
        {
            _layer = layer.GetDisplayName();
            _statusCode = (int)HttpStatusCode.InternalServerError;
            _service = service;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoxException"/> class with a layer, a specified error message, and status code.
        /// </summary>
        /// <param name="layer">The layer of the exception, representing the layer where the exception is thrown.</param>
        /// <param name="service">The service of the exception, representing the service where the exception is thrown.</param>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="statusCode">The HTTP status code associated with the exception.</param>
        protected NoxException(Enum layer, string service, string message, int statusCode)
            : base(message)
        {
            _layer = layer.GetDisplayName();
            _statusCode = statusCode;
            _service = service;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoxException"/> class with a layer, a specified error message, and a reference to the inner exception that is the cause of this exception.
        /// status code will be 500.
        /// </summary>
        /// <param name="layer">The layer of the exception, representing the layer where the exception is thrown.</param>
        /// <param name="service">The service of the exception, representing the service where the exception is thrown.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        protected NoxException(Enum layer, string service, string message, Exception innerException)
            : base(message, innerException)
        {
            _layer = layer.GetDisplayName();
            _statusCode = (int)HttpStatusCode.InternalServerError;
            _service = service;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoxException"/> class with a layer, a specified error message, and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="layer">The layer of the exception, representing the layer where the exception is thrown.</param>
        /// <param name="service">The service of the exception, representing the service where the exception is thrown.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        protected NoxException(Enum layer, string service, string message, int statusCode, Exception innerException)
            : base(message, innerException)
        {
            _layer = layer.GetDisplayName();
            _statusCode = statusCode;
            _service = service;
        }

        #endregion
    }
}
```

</details>

<br>

`NoxApplicationException:`

<details>
  <summary>Click to expand <i>NoxApplicationException</i></summary>

```cs
namespace AppyNox.Services.Base.Application.ExceptionExtensions.Base
{
    /// <summary>
    /// Represents exceptions specific to the application layer of the application.
    /// </summary>
    public class NoxApplicationException : NoxException, INoxApplicationException
    {
        #region [ Public Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="NoxApplicationException"/> class with a specific error message.
        /// Http status code is set to 500.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public NoxApplicationException(string message, string service = "Base")
            : base(ExceptionThrownLayer.Application, service, message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoxApplicationException"/> class with a specific error message and status code.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="statusCode">The HTTP status code associated with the exception.</param>
        public NoxApplicationException(string message, int statusCode, string service = "Base")
            : base(ExceptionThrownLayer.Application, service, message, statusCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoxApplicationException"/> class with an inner exception and an optional message.
        /// Http status code is set to 500.
        /// </summary>
        /// <param name="ex">The inner exception.</param>
        /// <param name="message">The message that describes the error. Defaults to "Unexpected error" if not provided.</param>
        public NoxApplicationException(Exception ex, string message = "Unexpected error", string service = "Base")
            : base(ExceptionThrownLayer.Application, service, message, ex)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoxApplicationException"/> class with an inner exception and an optional message with StatusCode.
        /// </summary>
        /// <param name="ex">The inner exception.</param>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="statusCode">The HTTP status code associated with the exception.</param>
        /// <param name="service">The service of the exception, representing the service where the exception is thrown.</param>
        public NoxApplicationException(Exception ex, string message, int statusCode, string service = "Base")
            : base(ExceptionThrownLayer.Application, service, message, statusCode, ex)
        {
        }

        #endregion
    }
}
```

</details>

<br>

`AccessTypeNotFoundException:`

<details>
    <summary>Click to expand <i>AccessTypeNotFoundException</i></summary>

```cs
namespace AppyNox.Services.Base.Application.ExceptionExtensions
{
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
            : base($"This '{entity.FullName}' entity has no access level mapping.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessTypeNotFoundException"/> class for a specific entity type and access type.
        /// Http status code is set to 400.
        /// </summary>
        /// <param name="entity">The entity type for which the access level mapping is not found.</param>
        /// <param name="accessType">The specific access type that is not found.</param>
        internal AccessTypeNotFoundException(Type entity, string accessType)
            : base($"This '{entity.FullName}' entity has no access level mapping for '{accessType}'.", (int)HttpStatusCode.BadRequest)
        {
        }

        #endregion
    }
}
```

</details>

**Important!**
It is important to remember `_service` variable is important for exceptions. For example `Base` comes from `NoxApiException` service parameter in ctors.

<details>
<summary>Click to expand</summary>

```json
{
  "version": "1.0",
  "isError": true,
  "responseException": {
    "error": {
      "code": 400,
      "title": "Base Nox Api",
      "message": "'Update2' is not a valid Access modifier.",
      "correlationId": "480d83ae-d334-4948-ba5d-bf0c952ef7a7"
    }
  }
}
```

```cs
namespace AppyNox.Services.Coupon.WebAPI.ExceptionExtensions.Base
{
    internal class NoxCouponApiException : NoxApiException
    {
        #region [ Fields ]

        private const string _service = "Coupon";

        #endregion

        #region [ Public Constructors ]

        public NoxCouponApiException(string message) : base(message, _service)
        {
        }

        public NoxCouponApiException(string message, int statusCode)
            : base(message, statusCode, _service)
        {
        }

        public NoxCouponApiException(Exception ex, string message = "Unexpected error")
            : base(ex, message, _service)
        {
        }

        public NoxCouponApiException(Exception ex, string message, int statusCode)
            : base(ex, message, statusCode, _service)
        {
        }

        #endregion
    }
}
```

As it is shown in NoxCouponApiException, service parameter should not be acceptes as a parameter of NoxCouponApiException ctors, it should be passed from the local variable of `_service`.

```json
{
  "version": "1.0",
  "isError": true,
  "responseException": {
    "error": {
      "code": 500,
      "title": "Coupon Nox Api",
      "message": "test exception",
      "correlationId": "e17977da-1823-4f31-8f6c-09de4c9d77b2"
    }
  }
}
```

</details>

<br>

**Additional Information**

For the ones who wants to learn where these exceptions are caught and wrapped, check the codes below:

<details>
  <summary>Click to expand <i>ExceptionHandlingMiddleware</i></summary>

```cs
namespace AppyNox.Services.Base.API.Middleware
{
    /// <summary>
    /// Middleware for handling exceptions that occur in the request pipeline.
    /// </summary>
    public class ExceptionHandlingMiddleware(RequestDelegate next, INoxApiLogger logger)
    {
        #region [ Fields ]

        private readonly RequestDelegate _next = next ?? throw new ArgumentNullException(nameof(next));

        private readonly INoxApiLogger _logger = logger;

        #endregion

        #region [ Public Methods ]

        /// <summary>
        /// Invokes the middleware to catch and handle exceptions during request processing.
        /// </summary>
        /// <param name="context">The HTTP context for the current request.</param>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (FluentValidationException ex)
            {
                string correlationId = CheckCorrelationIdsAndReturn(context, ex);

                ActionContext actionContext = new(context, context.GetRouteData(), new ControllerActionDescriptor());
                ModelStateDictionary modelState = new();
                ValidationHelpers.HandleValidationResult(modelState, ex.ValidationResult, actionContext);
                throw new ApiException(new NoxApiValidationExceptionWrapObject(ex, correlationId, modelState.AllErrors()), statusCode: ex.StatusCode);
            }
            catch (NoxException ex)
            {
                string correlationId = CheckCorrelationIdsAndReturn(context, ex);
                throw new ApiException(new NoxApiExceptionWrapObject(ex, correlationId), statusCode: ex.StatusCode);
            }
            catch (Exception ex)
            {
                throw new ApiException(ex, statusCode: 500);
            }
        }

        #endregion

        #region [ Private Methods ]

        /// <summary>
        /// Checks the CorrelationId from the request header and exception property.
        /// <para>NoxExceptionProperty of correlation id is set from CorrelationContext</para>
        /// </summary>
        private string CheckCorrelationIdsAndReturn(HttpContext context, NoxException ex)
        {
            string correlationId = (context.Request.Headers["X-Correlation-ID"].FirstOrDefault() ?? string.Empty).ToString() ?? string.Empty;
            if (correlationId.Equals(ex.CorrelationId.ToString()))
            {
                return correlationId;
            }
            else
            {
                Guid tempCorrelationId = Guid.NewGuid();
                _logger.LogCritical(new Exception(), $"Correlation Ids mismatched!!! Header: '{correlationId}', CorrelationContext: {ex.CorrelationId}", tempCorrelationId);
                return $"Malformed Correlation Id. Please provide '{tempCorrelationId} to customer service.'";
            }
        }

        #endregion
    }
}
```

</details>
