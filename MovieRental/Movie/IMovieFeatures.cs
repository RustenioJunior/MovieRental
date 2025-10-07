namespace MovieRental.Movie;

public interface IMovieFeatures
{
	Movie Save(Movie movie);
    Task<List<Movie>> GetAll(); 
    Task<List<Movie>> GetPaginated(int page, int pageSize); 
}