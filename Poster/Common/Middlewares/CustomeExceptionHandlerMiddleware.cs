using System.Net;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Poster.Logic.Common.Exceptions.Api;

namespace Poster.Common.Middlewares;

public class CustomExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public CustomExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch(Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError;

        IDictionary<string, string[]>? result = null;

        switch (exception)
        {
            case CustomException customException:
                code = HttpStatusCode.NotFound;
                result = customException.Failures;
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        if (result.IsNullOrEmpty())
        {
            return Task.CompletedTask;
        }

        var errors = new List<string>();

        foreach (var error in result)
        {
            errors.AddRange(error.Value);
        }

        return context.Response.WriteAsync(JsonConvert.SerializeObject(errors));
    }
}

public static class CustomExceptionHandlerMiddlewareExtension
{
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<CustomExceptionHandlerMiddleware>();

        return builder;
    }
}