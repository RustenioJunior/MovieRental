namespace MovieRental.Movie
{
    public interface IMovieFeatures
    {
        Task<Movie> CreateMovieAsync(Movie movie);
        Task<Movie> GetMovieByIdAsync(int id);
        Task<List<Movie>> GetAllAsync();
    }
}