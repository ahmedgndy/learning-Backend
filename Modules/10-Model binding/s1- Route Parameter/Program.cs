using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
var app = builder.Build();

app.MapControllers();

//diff names
app.MapGet("/product-minimal-1/{id:int}", ([FromRoute(Name = "id")] int identifer) => $"{identifer}");

//Get  "/product-minimal-1/388?id=10 //here will ignore the route paramater and the the id = 10 becacuse i tell him to do this 
//if i remove the [FromQuery] it will take the 388 because it has higher piorty 
app.MapGet("/product-minimal-1/{id:int}", ([FromQuery] int id ) => "identifer");


app.Run();
