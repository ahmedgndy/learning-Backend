using Microsoft.Extensions.DependencyInjection;
public static class DependencyInjection
{
    //for registration payment methods
    public static IServiceCollection AddPaymentServices(this IServiceCollection services, IHostEnvironment env)
    {

        if (env.IsProduction())
        {
            services.AddScoped<IPaymentService, PayPalPaymentService>();
            services.AddSingleton<IPaymentService, StripePaymentService>();

        }
        else if (env.IsDevelopment())
        {
            services.AddTransient<IPaymentService, FakePaymentService>();

        }

        //factory 
        services.AddTransient<IPaymentFactory, PaymentFactory>();

        return services;

    }

    //for registration lifetime services
    public static IServiceCollection AddLifetimeServices(this IServiceCollection services)
    {
        services.AddScoped<ITransientService, PayPalPaymentService>();
        services.AddSingleton<ISingletonService, StripePaymentService>();

        return services;
    }

}
