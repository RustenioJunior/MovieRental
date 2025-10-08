namespace MovieRentalApp.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
    }

    public class CreateMovieRequest
    {
        public string Title { get; set; } = string.Empty;
    }
}