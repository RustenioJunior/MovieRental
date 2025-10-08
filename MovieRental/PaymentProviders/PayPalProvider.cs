namespace MovieRental.PaymentProviders
{
    public class PayPalProvider : IPaymentProvider
    {
        public string ProviderName => "PayPal";

        public async Task<PaymentResult> ProcessPaymentAsync(decimal amount, string paymentInfo)
        {
            // Simulação de processamento PayPal
            await Task.Delay(800); // Simula delay de rede

            if (string.IsNullOrEmpty(paymentInfo) || !paymentInfo.Contains("@"))
            {
                return new PaymentResult
                {
                    Success = false,
                    ErrorMessage = "Invalid email format"
                };
            }

            if (amount > 1000) // Limite simulado
            {
                return new PaymentResult
                {
                    Success = false,
                    ErrorMessage = "Amount exceeds PayPal limit"
                };
            }

            // Simulação: 5% de chance de falha para teste
            if (new Random().Next(1, 21) == 1) // 5% chance
            {
                return new PaymentResult
                {
                    Success = false,
                    ErrorMessage = "PayPal payment failed: Transaction declined"
                };
            }

            return new PaymentResult
            {
                Success = true,
                TransactionId = $"PPL_{Guid.NewGuid():N}"
            };
        }
    }
}