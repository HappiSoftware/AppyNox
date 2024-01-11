namespace AppyNox.Gateway.OcelotGateway.Middlewares
{
    public class OcelotCorrelationIdMiddleware
    {
        #region Fields

        private readonly RequestDelegate _next;

        #endregion

        #region Public Constructors

        public OcelotCorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        #endregion

        #region Public Methods

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue("X-Correlation-ID", out var correlationId)
                || !Guid.TryParse(correlationId, out _))
            {
                correlationId = Guid.NewGuid().ToString();
                context.Request.Headers["X-Correlation-ID"] = correlationId;
            }

            context.Response.OnStarting(() =>
            {
                context.Response.Headers["X-Correlation-ID"] = correlationId;
                return Task.CompletedTask;
            });

            await _next(context);
        }

        #endregion
    }
}