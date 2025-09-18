
using System.Threading.RateLimiting;
using System.Xml.Linq;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRateLimiter(configureOptions =>
{
    configureOptions.AddFixedWindowLimiter("fixed", RateLimiter =>
       {
           RateLimiter.PermitLimit = 2; // Allow 2 requests
           RateLimiter.Window = TimeSpan.FromSeconds(10); // Within a 10-second window

           RateLimiter.QueueLimit = 0; // No queueing, extra requests are rejected immediately.
       });

    configureOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests; // Set status code for rejected requests
});
//registers the core health check services with the application.
//: Run your application and navigate to http://localhost:[port]/health. You should see a simple "Healthy" response in your browser.
builder.Services.AddHealthChecks();
var app = builder.Build();

app.UseRateLimiter();
/// logs every request
app.UseRequestResponseLogging();

app.MapGet("/api/resources", () => "This api resource is rate limited")
.RequireRateLimiting("fixed");

app.MapHealthChecks("/health");

//run a branch of middleware when it is a sensitive request
app.UseWhen(
    ctx => ctx.Request.Path.StartsWithSegments("/sensitive"),
    branch =>
    {
        // make a middle ware for sensitive data
        branch.Use(async (context, next) =>
        {
            // Log the request path
            Console.WriteLine($"Sensitive request made to: {context.Request.Path}");
            await next(context);
        });
        
        branch.Run(async context =>
        {
            await context.Response.WriteAsync("Sensitive data accessed!");
        });
    }
    );

app.Map("/admin", adminApp =>
{
    adminApp.UseMiddleware<AdminMiddleware>();

    // terminal middleware
    adminApp.Run(async context =>
    {
        await context.Response.WriteAsync("Welcome to the admin area!");
    });
});
app.Run();
