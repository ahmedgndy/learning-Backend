var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/products/{id}", (int id) => $"{id}");

//catch more than route paramaters

app.MapGet("/date/{year}-{month}-{day}", (int year, int month, int day) => $"Date is {new DateOnly(year, month, day)}");

//default rout
//if the use specified a controller i will take it if not i will make Home as default  
app.MapGet("/{controller=Home}", (string? Controller) => Controller);

//optional 
app.MapGet("/user/{id?}", (int? id) => id is null ? "All userse " : id);

//abcd
app.MapGet("/a{b}c{d}", (string b, string d) => $"b: {b}, d: {d}"); //b: b d:d

app.MapGet("/single/{*slug}", (string slug) => $"Slug: {slug}"); //give me every thing /path that user enterd



app.Run();
