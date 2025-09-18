var builder = WebApplication.CreateBuilder(args);
// Add all services to deal with the controller 
builder.Services.AddControllers();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

// Map the controllers
app.MapControllers();

app.MapGet("/products" , () => new[] { "Product1",  "Product3" });

//display all endpoints on routing table 

app.MapGet("/endpoints", (IServiceProvider sp) =>
{
    var endpoints = sp.GetRequiredService<EndpointDataSource>().Endpoints.Select(ep => ep.DisplayName);
    return Results.Ok(endpoints);
});


app.Run();
