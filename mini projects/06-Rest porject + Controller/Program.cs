using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
//add services 
builder.Services.AddControllers(options => options.ReturnHttpNotAcceptable = true )//406 format not fount 
                        .AddXmlSerializerFormatters()
                        .AddJsonOptions(options =>
                                        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                        );
;

builder.Services.AddSingleton<ProductRepository>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapControllers();

app.Run();
