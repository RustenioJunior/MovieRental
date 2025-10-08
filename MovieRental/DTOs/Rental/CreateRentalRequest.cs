namespace MovieRental.DTOs.Rental
{
    public class CreateRentalRequest
    {
        public int CustomerId { get; set; }
        public int MovieId { get; set; }
        public int DaysRented { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
    }
}