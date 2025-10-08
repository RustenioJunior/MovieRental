using MovieRental.Data;
using MovieRental.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace MovieRental.Rental
{
    public class RentalFeatures : IRentalFeatures
    {
        private readonly MovieRentalDbContext _context;

        public RentalFeatures(MovieRentalDbContext context)
        {
            _context = context;
        }

        public async Task<Rental> SaveAsync(Rental rental)
        {
            // ✅ Usando as novas exceções
            var movie = await _context.Movies.FindAsync(rental.MovieId);
            if (movie == null)
                throw new MovieNotFoundException(rental.MovieId);

            var customer = await _context.Customers.FindAsync(rental.CustomerId);
            if (customer == null)
                throw new CustomerNotFoundException(rental.CustomerId);

            // Define a data de devolução
            rental.ReturnDate = rental.RentalDate.AddDays(rental.DaysRented);

            _context.Rentals.Add(rental);

            await _context.SaveChangesAsync();

            // Carrega as relações para retorno
            await _context.Entry(rental)
                .Reference(r => r.Movie)
                .LoadAsync();

            await _context.Entry(rental)
                .Reference(r => r.Customer)
                .LoadAsync();

            return rental;
        }

        public async Task<IEnumerable<Rental>> GetRentalsByCustomerNameAsync(string customerName)
        {
            return await _context.Rentals
                .Include(r => r.Customer)
                .Include(r => r.Movie)
                .Where(r => r.Customer.Name.Contains(customerName))
                .ToListAsync();
        }
    }
}