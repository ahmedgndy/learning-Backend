namespace PaymentGatewayApi.Models
{ 
     public class PaymentRequest
    {
        public string Method { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "USD";
        public string? Description { get; set; }
    }
    
}