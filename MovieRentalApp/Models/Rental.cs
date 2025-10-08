namespace MovieRentalApp.Models
{
    public class Rental
    {
        public int Id { get; set; }
        public int DaysRented { get; set; }
        public string MovieTitle { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public DateTime RentalDate { get; set; }
        public DateTime ExpectedReturnDate { get; set; }
        public bool IsOverdue { get; set; }
        public int DaysUntilReturn { get; set; }

        public string TransactionId = $"TXN_{DateTime.UtcNow:yyyyMMddHHmmss}_{Guid.NewGuid().ToString("N").Substring(0, 6)}";

    }

    public class CreateRentalRequest
    {
        public int CustomerId { get; set; }
        public int MovieId { get; set; }
        public int DaysRented { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
    }
}