using MovieRental.Rental;
using Microsoft.AspNetCore.Mvc;
using MovieRental.Customer;
using MovieRental.DTOs.Rental;
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
            // validations with specific exceptions
            if (request == null)
                throw new InvalidRentalRequestException("Rental request cannot be null");

            if (request.CustomerId <= 0)
                throw new InvalidRentalRequestException("CustomerId is required and must be greater than 0");

            if (request.MovieId <= 0)
                throw new InvalidRentalRequestException("MovieId is required and must be greater than 0");

            if (request.DaysRented <= 0)
                throw new InvalidDaysRentedException(request.DaysRented);

            if (string.IsNullOrWhiteSpace(request.PaymentMethod))
                throw new InvalidPaymentMethodException(request.PaymentMethod);

            // verify if customer exists
            var customer = await _customerFeatures.GetCustomerByIdAsync(request.CustomerId);

            if (customer == null)
                throw new CustomerNotFoundException(request.CustomerId);

            var rental = new Rental.Rental
            {
                CustomerId = request.CustomerId,
                MovieId = request.MovieId,
                DaysRented = request.DaysRented,
                PaymentMethod = request.PaymentMethod
            };

            var result = await _features.SaveAsync(rental);

            var response = new RentalResponse
            {
                Id = result.Id,
                DaysRented = result.DaysRented,
                MovieTitle = result.Movie?.Title ?? "Unknown",
                CustomerName = result.Customer?.Name ?? "Unknown Customer",
                PaymentMethod = result.PaymentMethod,
                RentalDate = result.RentalDate
            };

            return Ok(response);
        }

        [HttpGet("customer/{customerName}")]
        public async Task<IActionResult> GetRentalsByCustomerName(string customerName)
        {
            // ✅ Validação com exceção específica
            if (string.IsNullOrWhiteSpace(customerName))
                throw new CustomerNameRequiredException();

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
    }
}