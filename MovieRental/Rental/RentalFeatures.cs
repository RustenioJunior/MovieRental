using Microsoft.EntityFrameworkCore;
using MovieRental.Data;
using MovieRental.PaymentProviders;
using MovieRental.Exceptions;

namespace MovieRental.Rental
{
	public class RentalFeatures : IRentalFeatures
	{
		private readonly MovieRentalDbContext _movieRentalDb;
        private readonly MovieRentalDbContext _context;
        private readonly ILogger<RentalFeatures> _logger;
        private readonly IPaymentProviderFactory _paymentFactory;

        public RentalFeatures(
            MovieRentalDbContext context,
            ILogger<RentalFeatures> logger,
            IPaymentProviderFactory paymentFactory)
        {
            _context = context;
            _logger = logger;
            _paymentFactory = paymentFactory;
        }

        public async Task<Rental> SaveAsync(Rental rental)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // ✅ Validar customer
                var customer = await _context.Customers.FindAsync(rental.CustomerId);
                if (customer == null)
                    throw new CustomerNotFoundException(rental.CustomerId);

                // ✅ Validar movie
                var movie = await _context.Movies.FindAsync(rental.MovieId);
                if (movie == null)
                    throw new MovieNotFoundException(rental.MovieId);

                // ✅ CALCULAR PREÇO (exemplo simples)
                decimal rentalPrice = CalculateRentalPrice(rental.DaysRented);

                // ✅ PROCESSAR PAGAMENTO
                var paymentResult = await ProcessPaymentAsync(rental.PaymentMethod, rentalPrice, customer);

                if (!paymentResult.Success)
                {
                    throw new PaymentFailedException(paymentResult.ErrorMessage);
                }

                // ✅ SALVAR RENTAL apenas se pagamento for bem-sucedido
                rental.RentalDate = DateTime.UtcNow;
                rental.TransactionId = paymentResult.TransactionId;
                rental.AmountPaid = rentalPrice;

                _context.Rentals.Add(rental);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation(
                    "Rental {RentalId} created successfully. Transaction: {TransactionId}",
                    rental.Id, paymentResult.TransactionId);

                return rental;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private async Task<PaymentResult> ProcessPaymentAsync(string paymentMethod, decimal amount, Customer.Customer customer)
        {
            try
            {
                var provider = _paymentProviderFactory.GetProvider(paymentMethod);
                _logger.LogInformation(
                    "Processing {Amount:C} payment via {Provider} for customer {CustomerName}",
                    amount, provider.ProviderName, customer.Name);

                // Info de pagamento baseada no provider
                string paymentInfo = paymentMethod.ToLower() switch
                {
                    "mbway" => customer.Phone, // MBWay usa telefone
                    "paypal" => customer.Email, // PayPal usa email
                    "credit" or "debit" => "card_token_123", // Cartão usaria token
                    _ => throw new ArgumentException($"Unsupported payment method: {paymentMethod}")
                };

                var result = await provider.ProcessPaymentAsync(amount, paymentInfo);

                if (result.Success)
                {
                    _logger.LogInformation(
                        "Payment successful. Transaction: {TransactionId}",
                        result.TransactionId);
                }
                else
                {
                    _logger.LogWarning(
                        "Payment failed for customer {CustomerName}: {Error}",
                        customer.Name, result.ErrorMessage);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment via {PaymentMethod}", paymentMethod);
                return new PaymentResult
                {
                    Success = false,
                    ErrorMessage = $"Payment processing error: {ex.Message}"
                };
            }
        }

        private decimal CalculateRentalPrice(int daysRented)
        {
            // Lógica simples de preço: R$ 5,00 por dia
            const decimal dailyRate = 5.00m;
            return daysRented * dailyRate;
        }

        //TODO: finish this method and create an endpoint for it //done
        public async Task<IEnumerable<Rental>> GetRentalsByCustomerNameAsync(string customerName)
        {
            if (string.IsNullOrWhiteSpace(customerName))    
                return Enumerable.Empty<Rental>();

            return await _movieRentalDb.Rentals
            .Include(r => r.Customer) 
            .Include(r => r.Movie)    
            .Where(r => r.Customer.Name.Contains(customerName)) 
            .OrderByDescending(r => r.RentalDate)
            .ToListAsync();
        }

    }
}
