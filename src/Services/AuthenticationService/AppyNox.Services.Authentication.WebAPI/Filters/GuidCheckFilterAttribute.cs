using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Text;
using AppyNox.Services.Authentication.Application.DTOs;

public class GuidCheckFilterAttribute : ActionFilterAttribute
{
    public List<string> RouteParameters { get; set; }
    public GuidCheckFilterAttribute()
    {
        RouteParameters = new List<string> { "id" };
    }
    public GuidCheckFilterAttribute(string[] routeParameters)
    {
        RouteParameters = new List<string>(routeParameters);
    }
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // Get the route data to access the URL parameters
        var routeData = context.HttpContext.GetRouteData();

        // Check and handle the GUID in the URL path
        foreach (var parameter in RouteParameters)
        {
            if (routeData.Values.TryGetValue(parameter, out var guidValue) && guidValue is string guidString)
            {
                if (!Guid.TryParse(guidString, out _))
                {
                    context.Result = new BadRequestObjectResult("Invalid GUID in URL path.");
                    return;
                }
            }
        }

        // Ensure the request body can be read multiple times
        context.HttpContext.Request.EnableBuffering();

        // Check and handle the GUID in the request body
        if (context.HttpContext.Request.ContentLength > 0 && context.HttpContext.Request.Body.CanRead)
        {
            // Read the request body stream and deserialize it using JsonSerializer
            using (StreamReader reader = new StreamReader(context.HttpContext.Request.Body, Encoding.UTF8))
            {
                context.HttpContext.Request.Body.Seek(0, SeekOrigin.Begin);
                string requestBody = await reader.ReadToEndAsync();

                if (!string.IsNullOrEmpty(requestBody))
                {
                    try
                    {
                        var options = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };

                        var yourObject = JsonSerializer.Deserialize<GuidDTO>(requestBody, options);

                        // Check and handle the GUID in the request body (similar to previous code)
                        if (yourObject != null)
                        {
                            if (!Guid.TryParse(yourObject.Id, out _))
                            {
                                context.Result = new BadRequestObjectResult("Invalid GUID in request body.");
                                return;
                            }
                        }
                    }
                    catch (JsonException)
                    {
                        context.Result = new BadRequestObjectResult("Invalid JSON format in the request body.");
                        return;
                    }
                }
            }
        }

        await next();
    }
}
