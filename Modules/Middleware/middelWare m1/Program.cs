var builder = WebApplication.CreateBuilder(args);
// Di container
var app = builder.Build();
// middleware setup

//01 middleware do noting 
app.Use((RequestDelegate next) => next);

//02 middleware intercept  httpContext object 

app.Use((RequestDelegate next) => //✅ input delegate here
{
    return async (HttpContext context) => // ✅ this entire lambda is the output delegate
    {
        await context.Response.WriteAsync("Hello from 02 middleware \n");
        await next(context);//  // calling input delegate
    };
});

//03 using extension method
app.Use(async (HttpContext context, RequestDelegate next) =>
{
    await context.Response.WriteAsync("Hello from 03 MW(HttpContext , RequestDelegate next ) \n");
    await next(context);
});

//app.run

app.Run(async (HttpContext context) =>
{
    await context.Response.WriteAsync("Hello from app.run \n");
});
//anything after that will never execute

app.Run();
