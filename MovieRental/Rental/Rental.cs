using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CustomerEntity = MovieRental.Customer.Customer;  
using MovieEntity = MovieRental.Movie.Movie;          

namespace MovieRental.Rental
{
    public class Rental
    {
        public Rental()
        {
            TransactionId = $"TXN_{DateTime.UtcNow:yyyyMMddHHmmss}";
        }

        [Key]
        public int Id { get; set; }
        public int DaysRented { get; set; }

        public MovieEntity? Movie { get; set; }  

        [ForeignKey("MovieId")]
        public int MovieId { get; set; }

        [Required]
        [RegularExpression("^(mbway|paypal|credit|debit)$", ErrorMessage = "Invalid payment method")]
        public string PaymentMethod { get; set; } = string.Empty;

        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public CustomerEntity Customer { get; set; } = null!; 

        public DateTime RentalDate { get; set; } = DateTime.UtcNow;
        public DateTime? ReturnDate { get; set; }

        public string TransactionId { get; set; }
        public decimal AmountPaid { get; set; }
        public bool PaymentProcessed { get; set; }
    }
}