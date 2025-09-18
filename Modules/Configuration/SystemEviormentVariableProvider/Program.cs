var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("MyCustomeCofig.json", optional: false, reloadOnChange: true);


var configData = new Dictionary<string, string?>
{
    { "apiKey", builder.Configuration["APIKEY"] },
    { "servicename", builder.Configuration["serviceName"] }
};
var app = builder.Build();

    
//go to configuration and get this key
app.MapGet("/{key}", (string key , IConfiguration config) => {

    return config[key] ?? "Not Found";
});


app.Run();

