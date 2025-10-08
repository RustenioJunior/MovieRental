namespace MovieRental.DTOs.Rental
{
    public class RentalResponse
    {
        public int Id { get; set; }
        public int DaysRented { get; set; }
        public string MovieTitle { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public DateTime RentalDate { get; set; } = DateTime.UtcNow;
    }
}