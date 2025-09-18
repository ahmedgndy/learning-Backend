using System.Text.Json.Serialization;
using M02.BuildingRESTFulAPI.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddSingleton<ProductRepository>();

var app = builder.Build();

app.MapControllers();

app.Run();