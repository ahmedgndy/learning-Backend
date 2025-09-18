using PaymentGatewayApi.Models;

class FakePaymentService : IPaymentService
{
    public string Name => "fake";
    public string PaymentId => Guid.NewGuid().ToString();
    public PaymentResponse PaymentProcess(PaymentRequest request)
    {
        // Implement fake payment processing logic here
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

     
