
using PaymentGatewayApi.Models;

public interface IPaymentService
{
   PaymentResponse PaymentProcess(PaymentRequest request);
   string Name { get; }

   string PaymentId { get; }
}