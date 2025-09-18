using System.Runtime.InteropServices.ComTypes;
var builder = WebApplication.CreateBuilder(args);

// cleaner way 
builder.Services.AddDependencyInjection();

//multiple registrations
builder.Services.AddTransient<IDependency, Dependencyv1>();
builder.Services.AddTransient<IDependency, Dependencyv2>();


var app = builder.Build();

app.MapGet("/weather/city/{cityName}", (string cityName, IWeatherService weatherService) =>
{
    string weatherInfo = weatherService.GetWeatherInfo(cityName);
    return Results.Ok(weatherInfo);
});

app.MapGet("/single", (IEnumerable<IDependency> dependencies) =>
{
    foreach (var dependency in dependencies)
    {
        Console.WriteLine(dependency.DoSomething());
    }   
});

app.MapGet("/idependency-registration", (IServiceProvider sp) =>
{
    var dependencies = sp.GetServices<IDependency>();
    foreach (var dependency in dependencies)
    {
        Console.WriteLine(dependency.DoSomething());
    }
});
()
app.Run();

interface IWeatherService
{
    string GetWeatherInfo(string cityName);
}

class WeatherService : IWeatherService
{
    private readonly IWeatherClient _weatherClient;

    // Dependency injection is used to inject the IWeatherClient
    // Constructor for WeatherService
    public WeatherService(IWeatherClient weatherClient)
    {
        _weatherClient = weatherClient;
    }

    public string GetWeatherInfo(string cityName)
    {
        return _weatherClient.GetWeatherInfo(cityName);
    }
}

interface IWeatherClient
{
    string GetWeatherInfo(string cityName);
}
//low level
class WeatherClient : IWeatherClient



{
    public string GetWeatherInfo(string cityName)
    {
        // Simulate fetching weather data from an external API
        return $"Weather for {cityName} is {Random.Shared.Next(-10, 40)} C.";
    }
}


/// 

public interface IDependency
{
    string DoSomething();
}

class Dependencyv1 : IDependency
{
    public string DoSomething()
    {
        return "Doing something! v1";
    }
}

class Dependencyv2 : IDependency
{
    public string DoSomething()
    {
        return "Doing something else! v2";
    }
}