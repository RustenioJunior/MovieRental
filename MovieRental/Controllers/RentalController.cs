using MovieRental.Rental;
using Microsoft.AspNetCore.Mvc;
using MovieRental.Customer;
using MovieRental.DTOs.Rental;    // Import centralized DTOs 
using MovieRental.Exceptions;

namespace MovieRental.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RentalController : ControllerBase
    {
        private readonly IRentalFeatures _features;
        private readonly ICustomerFeatures _customerFeatures;

        public RentalController(IRentalFeatures features, ICustomerFeatures customerFeatures)
        {
            _features = features;
            _customerFeatures = customerFeatures;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRental([FromBody] CreateRentalRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var customer = await _customerFeatures.GetCustomerByIdAsync(request.CustomerId);
                if (customer == null)
                    return BadRequest($"Customer with ID {request.CustomerId} not found");

                var rental = new Rental.Rental
                {
                    CustomerId = request.CustomerId,
                    MovieId = request.MovieId,
                    DaysRented = request.DaysRented,
                    PaymentMethod = request.PaymentMethod,
                    RentalDate = DateTime.UtcNow
                };

                var result = await _features.SaveAsync(rental);

                var response = new RentalResponse
                {
                    Id = result.Id,
                    DaysRented = result.DaysRented,
                    MovieTitle = result.Movie?.Title ?? "Unknown",
                    CustomerName = result.Customer.Name,
                    PaymentMethod = result.PaymentMethod,
                    RentalDate = result.RentalDate
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating rental: {ex.Message}");
            }
        }

        [HttpGet("customer/{customerName}")]
        public async Task<IActionResult> GetRentalsByCustomerName(string customerName)
        {
            try
            {
                var rentals = await _features.GetRentalsByCustomerNameAsync(customerName);

                if (!rentals.Any())
                    return NotFound($"No rentals found for customer: {customerName}");

                var response = rentals.Select(r => new RentalResponse
                {
                    Id = r.Id,
                    DaysRented = r.DaysRented,
                    MovieTitle = r.Movie?.Title ?? "Unknown",
                    CustomerName = r.Customer?.Name ?? "Unknown Customer",
                    PaymentMethod = r.PaymentMethod,
                    RentalDate = r.RentalDate
                });

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving rentals: {ex.Message}");
            }
        }
    }
}