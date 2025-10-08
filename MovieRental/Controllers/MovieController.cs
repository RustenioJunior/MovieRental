using Microsoft.AspNetCore.Mvc;
using MovieRental.Movie;
using MovieRental.DTOs.Movie;

namespace MovieRental.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieFeatures _features;

        public MoviesController(IMovieFeatures features)
        {
            _features = features;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovieById(int id)
        {
            var movie = await _features.GetMovieByIdAsync(id);

            var response = new MovieResponse
            {
                Id = movie.Id,
                Title = movie.Title
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMovie([FromBody] CreateMovieRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var movie = new Movie.Movie
            {
                Title = request.Title
            };

            var result = await _features.SaveAsync(movie);

            var response = new MovieResponse
            {
                Id = result.Id,
                Title = result.Title
            };

            return CreatedAtAction(nameof(GetMovieById), new { id = result.Id }, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMovies()
        {
            var movies = await _features.GetAllAsync();

            var response = movies.Select(m => new MovieResponse
            {
                Id = m.Id,
                Title = m.Title
            });

            return Ok(response);
        }
    }
}