var builder = WebApplication.CreateBuilder(args);
//Di container -> registration services 
builder.Services.AddSingleton<ProductRepository>();
var app = builder.Build();

app.MapGet("/text", () => "Hello World!"); //string

app.MapGet("/json", () => new { message = "hi" });//object

app.MapProductEndpoints();
app.Run();
