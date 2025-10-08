using Microsoft.Extensions.DependencyInjection;

namespace MovieRental.PaymentProviders
{
    public interface IPaymentProviderFactory
    {
        IPaymentProvider GetProvider(string paymentMethod);
    }

    public class PaymentProviderFactory : IPaymentProviderFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public PaymentProviderFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IPaymentProvider GetProvider(string paymentMethod)
        {
            return paymentMethod?.ToLower() switch
            {
                "mbway" => _serviceProvider.GetRequiredService<MbWayProvider>(),
                "paypal" => _serviceProvider.GetRequiredService<PayPalProvider>(),
                "credit" => _serviceProvider.GetRequiredService<CreditCardProvider>(), 
                "debit" => _serviceProvider.GetRequiredService<DebitCardProvider>(),   
                _ => throw new ArgumentException($"Unsupported payment method: {paymentMethod}")
            };
        }
    }
}