public class AdminMiddleware
{
    private readonly RequestDelegate _next;

    public AdminMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Custom logic for admin area
        await context.Response.WriteAsync("Welcome to the admin area!");
        // Call the next middleware in the pipeline
        await _next(context);
    }
}