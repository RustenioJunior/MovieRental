using System;
using System.Threading.Tasks;

namespace MovieRental.PaymentProviders
{
    public class DebitCardProvider : IPaymentProvider
    {
        public string ProviderName => "Debit Card";

        public async Task<PaymentResult> ProcessPaymentAsync(decimal amount, string paymentInfo)
        {
            // Simula processamento assíncrono
            await Task.Delay(800);

            try
            {
                // basic validation
                if (string.IsNullOrEmpty(paymentInfo))
                {
                    return new PaymentResult
                    {
                        Success = false,
                        ErrorMessage = "Card information is required"
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

                // ✅ VALIDAÇÃO ESPECÍFICA DE CARTÃO (simplificada)
                if (!IsValidCardNumber(paymentInfo))
                {
                    return new PaymentResult
                    {
                        Success = false,
                        ErrorMessage = "Invalid debit card number"
                    };
                }

                // ✅ VALIDAÇÃO DE LIMITE (cartão de débito)
                if (amount > 500) // Limite diário simulado
                {
                    return new PaymentResult
                    {
                        Success = false,
                        ErrorMessage = "Amount exceeds daily debit limit"
                    };
                }

                // ✅ SIMULAÇÃO DE FALHA (5% de chance)
                if (new Random().Next(1, 21) == 1) // 5% chance
                {
                    return new PaymentResult
                    {
                        Success = false,
                        ErrorMessage = "Debit card payment declined: Insufficient funds"
                    };
                }

                // ✅ PAGAMENTO BEM-SUCEDIDO
                return new PaymentResult
                {
                    Success = true,
                    TransactionId = $"DEB_{Guid.NewGuid():N}_{DateTime.Now:yyyyMMddHHmmss}"
                };
            }
            catch (Exception ex)
            {
                return new PaymentResult
                {
                    Success = false,
                    ErrorMessage = $"Debit card processing error: {ex.Message}"
                };
            }
        }

        // ✅ MÉTODO AUXILIAR - Valida número do cartão (simplificado)
        private bool IsValidCardNumber(string cardInfo)
        {
            // Formato esperado: "1234567812345678|12/25|123"
            var parts = cardInfo.Split('|');

            if (parts.Length < 3)
                return false;

            var cardNumber = parts[0].Replace(" ", "");

            // Verifica se tem 16 dígitos e é numérico
            return cardNumber.Length == 16 && long.TryParse(cardNumber, out _);
        }
    }
}