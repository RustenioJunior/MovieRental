using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using MovieRental.Movie;
using Microsoft.EntityFrameworkCore;
using MovieRental.Data;

namespace MovieRental.Movie
{
    public class MovieFeatures : IMovieFeatures
    {
        private readonly MovieRentalDbContext _movieRentalDb;
        public MovieFeatures(MovieRentalDbContext movieRentalDb)
        {
            _movieRentalDb = movieRentalDb;
        }

        public Movie Save(Movie movie)
        {
            _movieRentalDb.Movies.Add(movie);
            _movieRentalDb.SaveChanges();
            return movie;
        }

        // TODO: tell us what is wrong in this method? Forget about the async, what other concerns do you have?
        /// <summary>
        /// 1º:It will generate a list of all the movies in the database, and if there are too many movies, it can cause slowness and memory issues (or problems)
        /// 2º:There is no pagination or filtering, which can be problematic for large datasets.
        /// 3º: It does not handle potential exceptions that may occur during database access.
        /// 4º: Since the Movie class only contains Id and Title, there's no need for anything more complex at the moment, but if the class were to grow, we might want to consider using DTOs (Data Transfer Objects) to limit the data being transferred.
        /// </summary>
        /// <returns></returns>

        // without pagination
        public async Task<List<Movie>> GetAll()
        {
            return await _movieRentalDb.Movies
                .OrderBy(m => m.Title)
                .ToListAsync();
        }

        // with pagination
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
                // Error log
                Console.WriteLine($"Error retrieving movies: {ex.Message}");
                return new List<Movie>(); // Return empty list in case of error
            }

        }
    }
}

