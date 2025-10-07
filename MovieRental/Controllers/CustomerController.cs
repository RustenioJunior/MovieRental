using Microsoft.AspNetCore.Mvc;
using MovieRental.Customer;
using MovieRental.DTOs.Customer;

namespace MovieRental.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerFeatures _features;

        public CustomerController(ICustomerFeatures features)
        {
            _features = features;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // Convert DTO request to Entity    
                var customer = new Customer.Customer
                {
                    Name = request.Name,
                    Email = request.Email,
                    Phone = request.Phone
                };

                // call the feature to create the customer from interface
                var result = await _features.CreateCustomerAsync(customer);

                // Convert DTO response to Entity 
                var response = new CustomerResponse
                {
                    Id = result.Id,
                    Name = result.Name,
                    Email = result.Email,
                    Phone = result.Phone,
                    CreatedAt = result.CreatedAt
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating customer: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            try
            {
                var customer = await _features.GetCustomerByIdAsync(id);

                if (customer == null)
                    return NotFound($"Customer with ID {id} not found");

                var response = new CustomerResponse
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Email = customer.Email,
                    Phone = customer.Phone,
                    CreatedAt = customer.CreatedAt
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving customer: {ex.Message}");
            }
        }
    }
}