namespace PaymentGatewayApi.Models
{
    public class PaymentResponse
    {
        public string Status { get; set; } = "Failed";
        public string Provider { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string? TransactionId { get; set; }
        public string? Message { get; set; }
    }
}
