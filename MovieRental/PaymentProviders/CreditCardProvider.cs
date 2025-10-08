using System;
using System.Threading.Tasks;

namespace MovieRental.PaymentProviders
{
    public class CreditCardProvider : IPaymentProvider
    {
        public string ProviderName => "Credit Card";

        public async Task<PaymentResult> ProcessPaymentAsync(decimal amount, string paymentInfo)
        {
            // Simula processamento assíncrono
            await Task.Delay(700);

            try
            {
                // ✅ VALIDAÇÃO BÁSICA
                if (string.IsNullOrEmpty(paymentInfo))
                {
                    return new PaymentResult
                    {
                        Success = false,
                        ErrorMessage = "Credit card information is required"
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

                // ✅ VALIDAÇÃO ESPECÍFICA DE CARTÃO DE CRÉDITO
                if (!IsValidCreditCard(paymentInfo))
                {
                    return new PaymentResult
                    {
                        Success = false,
                        ErrorMessage = "Invalid credit card information"
                    };
                }

                // ✅ VALIDAÇÃO DE LIMITE (cartão de crédito é mais flexível)
                if (amount > 2000) // Limite mensal simulado
                {
                    return new PaymentResult
                    {
                        Success = false,
                        ErrorMessage = "Amount exceeds credit limit"
                    };
                }

                // ✅ VALIDAÇÃO DE PARCELAS (se aplicável)
                if (HasInstallments(paymentInfo) && amount < 100)
                {
                    return new PaymentResult
                    {
                        Success = false,
                        ErrorMessage = "Minimum amount for installments is $100"
                    };
                }

                // ✅ SIMULAÇÃO DE FALHA (8% de chance - crédito tem mais risco)
                if (new Random().Next(1, 13) == 1) // ~8% chance
                {
                    var failureReasons = new[]
                    {
                        "Credit card payment declined: Insufficient limit",
                        "Credit card payment declined: Suspicious activity",
                        "Credit card payment declined: Card blocked",
                        "Credit card payment declined: Expired card"
                    };

                    return new PaymentResult
                    {
                        Success = false,
                        ErrorMessage = failureReasons[new Random().Next(failureReasons.Length)]
                    };
                }

                // ✅ PAGAMENTO BEM-SUCEDIDO
                return new PaymentResult
                {
                    Success = true,
                    TransactionId = $"CRD_{Guid.NewGuid():N}_{DateTime.Now:yyyyMMddHHmmss}"
                };
            }
            catch (Exception ex)
            {
                return new PaymentResult
                {
                    Success = false,
                    ErrorMessage = $"Credit card processing error: {ex.Message}"
                };
            }
        }

        // ✅ MÉTODO AUXILIAR - Valida cartão de crédito
        private bool IsValidCreditCard(string cardInfo)
        {
            try
            {
                // Formato esperado: "1234567812345678|12/25|123|1" 
                // (número|validade|cvv|parcelas)
                var parts = cardInfo.Split('|');

                if (parts.Length < 3)
                    return false;

                var cardNumber = parts[0].Replace(" ", "");
                var expiry = parts[1];
                var cvv = parts[2];

                // Verifica número (16 dígitos)
                if (cardNumber.Length != 16 || !long.TryParse(cardNumber, out _))
                    return false;

                // Verifica CVV (3-4 dígitos)
                if (cvv.Length < 3 || cvv.Length > 4 || !int.TryParse(cvv, out _))
                    return false;

                // Verifica data de validade (MM/YY)
                if (!IsValidExpiryDate(expiry))
                    return false;

                return true;
            }
            catch
            {
                return false;
            }
        }

        // ✅ VALIDA DATA DE VALIDADE
        private bool IsValidExpiryDate(string expiry)
        {
            if (string.IsNullOrEmpty(expiry) || !expiry.Contains('/'))
                return false;

            var parts = expiry.Split('/');
            if (parts.Length != 2 || !int.TryParse(parts[0], out int month) ||
                !int.TryParse(parts[1], out int year))
                return false;

            // Ano com 2 dígitos → converte para 4 dígitos
            var fullYear = 2000 + year;
            var expiryDate = new DateTime(fullYear, month, 1).AddMonths(1).AddDays(-1);

            return expiryDate > DateTime.Now;
        }

        // ✅ VERIFICA SE TEM PARCELAS
        private bool HasInstallments(string cardInfo)
        {
            var parts = cardInfo.Split('|');
            return parts.Length >= 4 && int.TryParse(parts[3], out int installments) && installments > 1;
        }
    }
}