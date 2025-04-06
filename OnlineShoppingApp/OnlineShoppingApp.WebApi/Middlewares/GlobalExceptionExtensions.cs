namespace OnlineShoppingApp.WebApi.Middlewares;

public static class GlobalExceptionExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<GlobalExceptionMiddleware>();
    }
}