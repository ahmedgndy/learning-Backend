public static class CustomMiddlewareExtensions
{

    public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestResponseLoggingMiddleware>();
    }

    public static IApplicationBuilder UseAdminMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AdminMiddleware>();
    }

    
}

        
