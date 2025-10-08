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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            // ✅ ZERO try-catch! O middleware cuida de tudo
            var customer = await _features.GetCustomerByIdAsync(id);
            
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

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerRequest request)
        {
            // ✅ Apenas validação do ModelState
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // ✅ Lógica limpa - exceptions são tratadas pelo middleware
            var customer = new Customer.Customer
            {
                Name = request.Name,
                Email = request.Email,
                Phone = request.Phone
            };

            var result = await _features.CreateCustomerAsync(customer);
            
            var response = new CustomerResponse
            {
                Id = result.Id,
                Name = result.Name,
                Email = result.Email,
                Phone = result.Phone,
                CreatedAt = result.CreatedAt
            };

            return CreatedAtAction(nameof(GetCustomerById), new { id = result.Id }, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            // ✅ Também sem try-catch
            var customers = await _features.GetAllCustomersAsync();
            
            var response = customers.Select(c => new CustomerResponse
            {
                Id = c.Id,
                Name = c.Name,
                Email = c.Email,
                Phone = c.Phone,
                CreatedAt = c.CreatedAt
            });

            return Ok(response);
        }
    }
}