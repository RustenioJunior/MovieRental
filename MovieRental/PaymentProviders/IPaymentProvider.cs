namespace MovieRental.PaymentProviders
{
    public interface IPaymentProvider
    {
        string ProviderName { get; }
        Task<PaymentResult> ProcessPaymentAsync(decimal amount, string paymentInfo);
    }

    public class PaymentResult
    {
        public bool Success { get; set; }
        public string TransactionId { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime ProcessedAt { get; set; } = DateTime.UtcNow;
    }
}