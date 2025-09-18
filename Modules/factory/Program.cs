using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IPaymentProvider>(sp =>
{
    var paymentProvider = builder.Configuration["paymentProvider"];
    return paymentProvider switch
    {
        "stripe" => new StripePayment(),
        "paypal" => new PayPalPayment(),
        _ => throw new ArgumentException("Invalid payment provider specified.")
    };
});

var app = builder.Build();

app.MapGet("/pay/{amount}", (decimal amount, IPaymentProvider paymentProvider) =>
{
    return paymentProvider.Pay(amount);
});

app.Run();

public interface IPaymentProvider
{
    string Pay(decimal amount);
}

public class StripePayment : IPaymentProvider
{
    public string Pay(decimal amount)
    {
        // Implementation for Stripe payment processing
        return $"Processed payment of {amount} through Stripe.";
    }
}

public class PayPalPayment : IPaymentProvider
{
    public string Pay(decimal amount)
    {
        // Implementation for PayPal payment processing
        return $"Processed payment of {amount} through PayPal.";
    }
}