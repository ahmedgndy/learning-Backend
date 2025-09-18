using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

//register the weather settings
builder.Services.Configure<WeatherSettings>(builder.Configuration.GetSection("WeatherSettings"));  

var app = builder.Build();

app.MapGet("/get-weather", (IOptions<WeatherSettings> options) =>
{
    var weatherSettings = options.Value;

    var weatherData = new Dictionary<string, string>
    {
        { "City", weatherSettings.DefaultCity },
        { "TemperatureUnit", (30 + " " + weatherSettings.TemperatureUnit).ToString() }
    };
    return weatherData;

});


app.Run();

