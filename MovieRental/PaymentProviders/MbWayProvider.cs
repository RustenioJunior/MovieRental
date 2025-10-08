namespace MovieRental.PaymentProviders
{
    public class MbWayProvider : IPaymentProvider
    {
        public string ProviderName => "MBWay";

        public async Task<PaymentResult> ProcessPaymentAsync(decimal amount, string paymentInfo)
        {
            // Simulação de processamento MBWay
            await Task.Delay(1000); // Simula delay de rede

            // Lógica dummy existente adaptada
            if (string.IsNullOrEmpty(paymentInfo) || !paymentInfo.StartsWith("+"))
            {
                return new PaymentResult
                {
                    Success = false,
                    ErrorMessage = "Invalid phone number format"
                };
            }

            if (amount <= 0)
            {
                return new PaymentResult
                {
                    Success = false,
                    ErrorMessage = "Invalid amount"
                };
            }

            // Simulação: 10% de chance de falha para teste
            if (new Random().Next(1, 11) == 1) // 10% chance
            {
                return new PaymentResult
                {
                    Success = false,
                    ErrorMessage = "MBWay payment failed: Insufficient funds"
                };
            }

            return new PaymentResult
            {
                Success = true,
                TransactionId = $"MBW_{Guid.NewGuid():N}"
            };
        }
    }
}