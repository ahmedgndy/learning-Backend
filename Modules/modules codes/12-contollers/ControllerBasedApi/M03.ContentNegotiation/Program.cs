using M03.ContentNegotiation.Data;
using M03.ContentNegotiation.Formatters;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.ReturnHttpNotAcceptable = true; // 406 
    options.OutputFormatters.Add(new PlainTextTableOutputFormatter());
}).AddXmlSerializerFormatters();

builder.Services.AddSingleton<ProductRepository>();

var app = builder.Build();

app.MapControllers();

app.Run();

