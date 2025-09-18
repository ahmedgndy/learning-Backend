
namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            services.AddTransient<IWeatherService, WeatherService>();
            services.AddTransient<IWeatherClient, WeatherClient>();
            return services;

        }
    }
}