using AppyNox.Services.Base.Application.Extensions;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Domain.Common;
using Newtonsoft.Json;

namespace AppyNox.Gateway.OcelotGateway.Middlewares
{
    /// <summary>
    /// Middleware for logging HTTP requests and responses.
    /// </summary>
    public class LoggingMiddleware(RequestDelegate next, INoxApiLogger logger)
    {
        #region [ Fields ]

        private readonly RequestDelegate _next = next;

        private readonly INoxApiLogger _logger = logger;

        #endregion

        #region [ Public Methods ]

        /// <summary>
        /// Invokes the middleware operation for the current HTTP context.
        /// </summary>
        /// <param name="context">The current HTTP context.</param>
        public async Task Invoke(HttpContext context)
        {
            if (IsExcludedRoute(context.Request.Path))
            {
                await _next.Invoke(context);
                return;
            }
            await LogRequest(context);

            var originalResponseBody = context.Response.Body;

            try
            {
                using var responseBody = new MemoryStream();
                context.Response.Body = responseBody;
                await _next.Invoke(context);
                await LogResponse(context, responseBody, originalResponseBody);
            }
            finally
            {
                // Reset the original response body stream.
                context.Response.Body = originalResponseBody;
            }
        }

        #endregion

        #region [ Private Methods ]

        private static bool IsExcludedRoute(string route)
        {
            // Exclude swagger routes from logging.
            return route.Contains("swagger", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Logs the HTTP response content.
        /// </summary>
        /// <param name="context">The current HTTP context.</param>
        /// <param name="responseBody">The response body as a memory stream.</param>
        /// <param name="originalResponseBody">The original response body stream.</param>
        /// <remarks>
        /// This method reads the response content from a memory stream, logs it,
        /// and then copies it back to the original response body stream.
        /// </remarks>
        private async Task LogResponse(HttpContext context, MemoryStream responseBody, Stream originalResponseBody)
        {
            // Read and log the response body.
            responseBody.Position = 0;
            var content = await new StreamReader(responseBody).ReadToEndAsync();
            _logger.LogInformation(JsonConvert.SerializeObject(content).MinifyLogData());

            // Copy the logged response abck to the original response body stream.
            responseBody.Position = 0;
            await responseBody.CopyToAsync(originalResponseBody);
        }

        /// <summary>
        /// Logs the HTTP request content.
        /// </summary>
        /// <param name="context">The current HTTP context.</param>
        /// <remarks>
        /// This method reads and logs the request body. It enables buffering for the request
        /// stream to allow reading the content without affecting the request processing.
        /// </remarks>
        private async Task LogRequest(HttpContext context)
        {
            // Log the request body.
            context.Request.EnableBuffering();
            var requestReader = new StreamReader(context.Request.Body);
            var content = await requestReader.ReadToEndAsync();

            var requestData = new RequestLogModel(context.Request.Method.ToUpper(), context.Request.Path,
                                            string.Join(',', context.Request.Query.Select(q => $"{q.Key}:{q.Value}").ToList()), content);

            _logger.LogInformation(JsonConvert.SerializeObject(requestData).MinifyLogData());
            context.Request.Body.Position = 0;
        }

        #endregion
    }
}