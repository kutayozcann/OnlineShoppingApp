using System.Net;
using System.Text.Json;

namespace OnlineShoppingApp.WebApi.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, message) = exception switch
        {
            ApplicationException _ => (HttpStatusCode.BadRequest, "Process error occurred"),
            KeyNotFoundException _ => (HttpStatusCode.NotFound, "Key not found"),
            UnauthorizedAccessException _ => (HttpStatusCode.Unauthorized, "Access denied"),
            _ => (HttpStatusCode.InternalServerError, "Internal server error"),
        };

        context.Response.StatusCode = (int)statusCode;

        var errorResponse = new
        {
            StatusCode = context.Response.StatusCode,
            Message = message,
            Detail = exception.Message,
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
    }
}