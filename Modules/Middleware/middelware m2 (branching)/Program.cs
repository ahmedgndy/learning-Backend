var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapWhen(context => context.Request.Path.StartsWithSegments("/branch1"), b1 =>
{
    b1.Use(async (context, next) =>
    {
        await context.Response.WriteAsync("Branch 1 - Middleware 1\n");
        await next(context);
    });
    b1.Use(async (context, next) =>
    {
        await context.Response.WriteAsync("Branch 1 - Middleware 2\n");
        await next(context);
    });
    b1.Use(async (context, next) =>
    {
        await context.Response.WriteAsync("Branch 1 - Middleware 3\n");
        await  next(context);
    });
});


app.Map("/branch2", b2 =>
{
    b2.Use(async (context, next) =>
    {
        await context.Response.WriteAsync("Branch 2- Middleware 1\n");
        await next(context);
    });
    b2.Use(async (context, next) =>
    {
        await context.Response.WriteAsync("Branch 2 - Middleware 2\n");
        await next(context);
    });
    b2.Use(async (context, next) =>
    {
        await context.Response.WriteAsync("Branch 2 - Middleware 3\n");
        await  next(context);
    });
});

app.Run(async context =>
{
    await context.Response.WriteAsync("Hello from the root!");

});
app.Run();
