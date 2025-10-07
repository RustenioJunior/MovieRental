namespace MovieRental.DTOs.Rental
{
    public class RentalResponse
    {
        public int Id { get; set; }
        public int DaysRented { get; set; }
        public string MovieTitle { get; set; }
        public string CustomerName { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime ExpectedReturnDate => RentalDate.AddDays(DaysRented);

        public bool IsOverdue => DateTime.UtcNow > ExpectedReturnDate;
        public int DaysUntilReturn => (ExpectedReturnDate - DateTime.UtcNow).Days;
    }
}