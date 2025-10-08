using System.ComponentModel.DataAnnotations;

namespace MovieRental.DTOs.Movie
{
    public class CreateMovieRequest
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; }

        [Range(1900, 2100)]
        public int ReleaseYear { get; set; }

        [StringLength(50)]
        public string Genre { get; set; }
    }
}

public class MovieResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; }
    public int ReleaseYear { get; set; }
    public string Genre { get; set; }
    public bool IsAvailable { get; set; }
}