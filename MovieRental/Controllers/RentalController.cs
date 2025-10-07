using Microsoft.AspNetCore.Mvc;
using MovieRental.Movie;
using MovieRental.Rental;

namespace MovieRental.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RentalController : ControllerBase
    {

        private readonly IRentalFeatures _features;

        public RentalController(IRentalFeatures features)
        {
            _features = features;
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Rental.Rental rental)
        {
            var result = await _features.SaveAsync(rental);
            return Ok(result);
        }

        [HttpGet("customer/{customerName}")]
        public async Task<IActionResult> GetRentalsByCustomerName(string customerName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(customerName))
                {
                    return BadRequest("Customer name is required");
                }

                var rentals = await _features.GetRentalsByCustomerNameAsync(customerName);

                if (!rentals.Any())
                {
                    return NotFound($"No rentals found for customer: {customerName}");
                }

                return Ok(rentals);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving rentals: {ex.Message}");
            }
        }
    }
}
