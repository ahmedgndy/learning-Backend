var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/authors/{author}", static async (string author, HttpContext context) =>
{

    var data = new
    {
        Id = context.TraceIdentifier,
        Scheme = context.Request.Scheme,
        Host = context.Request.Host,
        Method = context.Request.Method,
        Path = context.Request.Path,
        Query = context.Request.Query,
        Headers = context.Request.Headers,
        RouteValues = context.Request.RouteValues,
        Body = await new StreamReader(context.Request.Body).ReadToEndAsync(),

    };
    return Results.Ok(data); 
}
);

app.Run();
