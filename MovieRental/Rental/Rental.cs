using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Customer = MovieRental.Customer.Customer;

namespace MovieRental.Rental
{
    public class Rental
    {
        [Key]
        public int Id { get; set; }
        public int DaysRented { get; set; }
        public Movie.Movie? Movie { get; set; }

        [ForeignKey("Movie")]
        public int MovieId { get; set; }

        // ✅ PaymentMethod agora determina qual provider usar
        [Required]
        [RegularExpression("^(mbway|paypal|credit|debit)$", ErrorMessage = "Invalid payment method")]
        public string PaymentMethod { get; set; }

        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public Customer.Customer Customer { get; set; }

        public DateTime RentalDate { get; set; } = DateTime.UtcNow;
        public DateTime? ReturnDate { get; set; }

        // ✅ Novos campos para pagamento
        public string TransactionId { get; set; }
        public decimal AmountPaid { get; set; }
        public bool PaymentProcessed { get; set; }
    }
}