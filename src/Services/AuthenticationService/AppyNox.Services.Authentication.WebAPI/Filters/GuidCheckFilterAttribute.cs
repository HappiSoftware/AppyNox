using AppyNox.Services.Authentication.Application.Dtos;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;

namespace AppyNox.Services.Authentication.WebAPI.Filters
{
    public class GuidCheckFilterAttribute : ActionFilterAttribute
    {
        #region [ Public Constructors ]

        public GuidCheckFilterAttribute()
        {
            RouteParameters = new List<string> { "id" };
        }

        public GuidCheckFilterAttribute(string routeParameters)
        {
            RouteParameters = routeParameters.Split(',').ToList();
        }

        #endregion

        #region [ Properties ]

        public List<string> RouteParameters { get; set; }

        #endregion

        #region [ Fields ]

        private static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        #endregion

        #region [ Public Methods ]

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (TryHandleGuidInUrlPath(context, context.HttpContext.GetRouteData()))
            {
                await next();
                return;
            }

            if (await TryHandleGuidInRequestBodyAsync(context, context.HttpContext.Request))
            {
                await next();
                return;
            }

            await next();
        }

        private static async Task<bool> TryHandleGuidInRequestBodyAsync(ActionExecutingContext context, HttpRequest request)
        {
            if (request.ContentLength > 0 && request.Body.CanRead)
            {
                // Ensure the request body can be read multiple times
                request.EnableBuffering();

                using (StreamReader reader = new StreamReader(request.Body, Encoding.UTF8))
                {
                    request.Body.Seek(0, SeekOrigin.Begin);
                    string requestBody = await reader.ReadToEndAsync();

                    if (!string.IsNullOrEmpty(requestBody))
                    {
                        try
                        {
                            var yourObject = JsonSerializer.Deserialize<GuidDto>(requestBody, _jsonSerializerOptions);

                            // Check and handle the GUID in the request body (similar to previous code)
                            if (yourObject != null && !Guid.TryParse(yourObject.Id, out _))
                            {
                                context.Result = new BadRequestObjectResult("Invalid GUID in request body.");
                                return true;
                            }
                        }
                        catch (JsonException)
                        {
                            context.Result = new BadRequestObjectResult("Invalid JSON format in the request body.");
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private bool TryHandleGuidInUrlPath(ActionExecutingContext context, RouteData routeData)
        {
            foreach (var parameter in RouteParameters)
            {
                if (routeData.Values.TryGetValue(parameter, out var guidValue) &&
                    guidValue is string guidString &&
                    !Guid.TryParse(guidString, out _))
                {
                    context.Result = new BadRequestObjectResult("Invalid GUID in URL path.");
                    return true;
                }
            }
            return false;
        }

        #endregion
    }
}