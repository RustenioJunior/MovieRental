using Microsoft.EntityFrameworkCore;
using MovieRental.Data;
using MovieRental.Exceptions;

namespace MovieRental.Movie
{
    public class MovieFeatures : IMovieFeatures
    {
        private readonly MovieRentalDbContext _movieRentalDb;

        public MovieFeatures(MovieRentalDbContext movieRentalDb)
        {
            _movieRentalDb = movieRentalDb;
        }

        public async Task<Movie> CreateMovieAsync(Movie movie)
        {
            _movieRentalDb.Movies.Add(movie);
            await _movieRentalDb.SaveChangesAsync();
            return movie;
        }

        public async Task<Movie> GetMovieByIdAsync(int id)
        {
            var movie = await _movieRentalDb.Movies.FindAsync(id);

            if (movie == null)
                throw new MovieNotFoundException(id); // ✅ Correto!

            return movie;
        }

        public async Task<List<Movie>> GetAllAsync()
        {
            return await _movieRentalDb.Movies
                .OrderBy(m => m.Title)
                .ToListAsync();
        }

        public async Task<List<Movie>> GetPaginated(int page = 1, int pageSize = 50)
        {
            try
            {
                if (page < 1) page = 1;
                if (pageSize > 100) pageSize = 100;
                if (pageSize < 1) pageSize = 10;

                return await _movieRentalDb.Movies
                    .OrderBy(m => m.Title)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving movies: {ex.Message}");
                return new List<Movie>();
            }
        }
    }
}