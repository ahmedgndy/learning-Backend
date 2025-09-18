using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JsonOptions>(options =>{
    options.SerializerOptions.DefaultIgnoreCondition =        System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
}
);
builder.Services.AddSingleton<ProductRepository>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapProductEndpoints();

app.Run();
