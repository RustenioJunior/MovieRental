namespace MovieRental.DTOs.Customer
{
    public class CustomerResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedAt { get; set; }   

        public int TotalRentals { get; set; }
        public DateTime? LastRentalDate { get; set; }
        public bool IsActive { get; set; }
    }
}