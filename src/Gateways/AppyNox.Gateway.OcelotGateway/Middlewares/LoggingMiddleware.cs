using AppyNox.Services.Base.Application.Helpers;
using AppyNox.Services.Base.Domain.Common;
using Newtonsoft.Json;

namespace AppyNox.Gateway.OcelotGateway.Middlewares
{
    public class LoggingMiddleware
    {
        #region [ Fields ]

        private readonly RequestDelegate _next;

        private readonly ILogger<LoggingMiddleware> _logger;

        #endregion

        #region [ Public Constructors ]

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        #endregion

        #region [ Public Methods ]

        public async Task Invoke(HttpContext context)
        {
            if (IsExcludedRoute(context.Request.Path))
            {
                await _next.Invoke(context);
                return;
            }
            await LogRequest(context);

            var originalResponseBody = context.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;
                await _next.Invoke(context);

                await LogResponse(context, responseBody, originalResponseBody);
            }
        }

        #endregion

        #region [ Private Methods ]

        private async Task LogResponse(HttpContext context, MemoryStream responseBody, Stream originalResponseBody)
        {
            responseBody.Position = 0;
            var content = await new StreamReader(responseBody).ReadToEndAsync();
            responseBody.Position = 0;
            await responseBody.CopyToAsync(originalResponseBody);
            context.Response.Body = originalResponseBody;
            var minifiedLogMessage = JsonConvert.SerializeObject(content).MinifyLogData();
            _logger.LogInformation("{LogData}", minifiedLogMessage);
        }

        private async Task LogRequest(HttpContext context)
        {
            context.Request.EnableBuffering();
            var requestReader = new StreamReader(context.Request.Body);
            var content = await requestReader.ReadToEndAsync();

            var requestData = new RequestLogModel(context.Request.Method.ToUpper(), context.Request.Path,
                                            string.Join(',', context.Request.Query.Select(q => $"{q.Key}:{q.Value}").ToList()), content);

            var minifiedLogMessage = JsonConvert.SerializeObject(requestData).MinifyLogData();
            _logger.LogInformation("{LogData}", minifiedLogMessage);
            context.Request.Body.Position = 0;
        }

        private static bool IsExcludedRoute(string route)
        {
            if (route.Contains("swagger"))
            {
                return true;
            }

            return false;
        }

        #endregion
    }
}