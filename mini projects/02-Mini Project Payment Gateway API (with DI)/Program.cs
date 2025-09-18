
using PaymentGatewayApi.Models;

var builder = WebApplication.CreateBuilder(args);

//registration the services 
builder.Services.AddPaymentServices(builder.Environment);
builder.Services.AddLifetimeServices();



var app = builder.Build();

app.MapPost("/pay/{paymentMethod}/{amount}", (string paymentMethod, decimal amount, IPaymentFactory paymentFactory ) =>
{
    try
    {
        var paymentService = paymentFactory.GetService(paymentMethod);
        var paymentRequest = new PaymentRequest
        {
            Amount = amount,
            Currency = "USD",
            Description = $"Payment for {paymentMethod}"
        };

        var paymentResponse = paymentService.PaymentProcess(paymentRequest);

        return Results.Ok(paymentResponse);
    }
    catch
    { 
        return Results.NotFound("Payment Provider Not Supported");
    }
});


app.MapGet("/di/lifTimes", (ITransientService transientService  , ISingletonService singletonService) =>
{
      return Results.Ok(
        new
        {
            Transient = transientService.Id,
            Singleton = singletonService.Id
        }
      );
});


app.Run();


