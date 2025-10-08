namespace MovieRentalApp.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
    }

    public class CreateMovieRequest
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; }
        public int ReleaseYear { get; set; }
        public string Genre { get; set; }
        public bool IsAvailable { get; set; }
    }
}