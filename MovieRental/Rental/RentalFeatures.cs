using Microsoft.EntityFrameworkCore;
using MovieRental.Data;

namespace MovieRental.Rental
{
	public class RentalFeatures : IRentalFeatures
	{
		private readonly MovieRentalDbContext _movieRentalDb;
		public RentalFeatures(MovieRentalDbContext movieRentalDb)
		{
			_movieRentalDb = movieRentalDb;
		}

        //TODO: make me async :( // done
        public async Task<Rental> SaveAsync(Rental rental)
        {
            _movieRentalDb.Rentals.Add(rental);
            await _movieRentalDb.SaveChangesAsync(); 
            return rental;
        }

        //TODO: finish this method and create an endpoint for it
        public async Task<IEnumerable<Rental>> GetRentalsByCustomerNameAsync(string customerName)
        {
            if (string.IsNullOrWhiteSpace(customerName))
                return Enumerable.Empty<Rental>();

            return await _movieRentalDb.Rentals
                .Where(r => r.CustomerName.Contains(customerName)) // Busca parcial
                .OrderByDescending(r => r.DaysRented) // Mais recentes primeiro
                .ToListAsync();
        }

    }
}
