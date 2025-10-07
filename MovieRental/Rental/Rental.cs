using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MovieRental.Customer;

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

		public string PaymentMethod { get; set; }

        // TODO: we should have a table for the customers
        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public MovieRental.Customer.Customer Customer { get; set; }
        public DateTime RentalDate { get; set; } = DateTime.UtcNow;
        public DateTime? ReturnDate { get; set; }
    }
}
