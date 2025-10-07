using System.ComponentModel.DataAnnotations;

namespace MovieRental.DTOs.Rental
{
    public class CreateRentalRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "CustomerId is required")]
        public int CustomerId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "MovieId is required")]
        public int MovieId { get; set; }

        [Range(1, 30, ErrorMessage = "Days rented must be between 1 and 30")]
        public int DaysRented { get; set; }

        [Required]
        [RegularExpression("^(credit|debit|paypal|mbway)$", ErrorMessage = "Invalid payment method")]
        public string PaymentMethod { get; set; }
    }
}