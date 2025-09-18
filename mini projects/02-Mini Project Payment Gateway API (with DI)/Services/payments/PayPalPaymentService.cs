using PaymentGatewayApi.Models;

class PayPalPaymentService : IPaymentService,ITransientService
{
    public string Name => "PayPal";
    public Guid Id { get; } = Guid.NewGuid();
    public string PaymentId => Guid.NewGuid().ToString();

    public PaymentResponse PaymentProcess(PaymentRequest request)
    {
        // Implement PayPal payment processing logic here
        return new PaymentResponse
        {
            Status = "Success",
            Provider = Name,
            Amount = request.Amount,
            TransactionId = Guid.NewGuid().ToString(),
            Message = "Payment processed successfully."
        };
    }
}

     

