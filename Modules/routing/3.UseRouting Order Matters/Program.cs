using System.Runtime.Intrinsics.X86;
var builder = WebApplication.CreateBuilder(args);


var app = builder.Build();

app.Use(async (context, next) =>
{
    var endpoint = context.GetEndpoint()?.DisplayName ?? "No Endpoint";
    Console.WriteLine($"MW #1 {endpoint}");
    await next();

});
//ording of middleware matters 
// if i use this here i will give me unexpected resulat 
app.useRouting(); 
app.Use(async (context, next) =>
{
    var endpoint = context.GetEndpoint()?.DisplayName ?? "No Endpoint";
    Console.WriteLine($"MW #2 {endpoint}");
    await next();

});

app.MapGet("/products", () =>
{

    return Results.Ok(new[] { "product1", "product2" });
}

 );


app.Run();
