using PaymentGatewayApi.Models;

class StripePaymentService : IPaymentService, ISingletonService
{
    public string Name => "Stripe";
    public Guid Id { get; } = Guid.NewGuid();

    public string PaymentId => Guid.NewGuid().ToString();
    public PaymentResponse PaymentProcess(PaymentRequest request)
    {
        // Implement Stripe payment processing logic here
        return new PaymentResponse
        {
            Status = "Success",
            Provider = Name,
            Amount = request.Amount,
            TransactionId = Guid.NewGuid().ToString(),
            Message = "Payment processed successfully."
        };
    }
};