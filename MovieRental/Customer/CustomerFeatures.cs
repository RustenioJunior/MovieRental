using Microsoft.EntityFrameworkCore;
using MovieRental.Data;
using MovieRental.Exceptions;
using MovieRentalCustomer = MovieRental.Customer.Customer;

namespace MovieRental.Customer
{
    public class CustomerFeatures : ICustomerFeatures
    {
        private readonly MovieRentalDbContext _context;

        public CustomerFeatures(MovieRentalDbContext context)
        {
            _context = context;
        }

        public async Task<MovieRentalCustomer> CreateCustomerAsync(MovieRentalCustomer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<MovieRentalCustomer> GetCustomerByIdAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
                throw new CustomerNotFoundException(id);
            return customer;
        }

        public async Task<IEnumerable<MovieRentalCustomer>> GetAllCustomersAsync()
        {
            return await _context.Customers.ToListAsync(); 
        }

        public async Task<MovieRentalCustomer> GetCustomerByEmailAsync(string email)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task<IEnumerable<MovieRentalCustomer>> GetCustomersByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Enumerable.Empty<MovieRentalCustomer>();

            return await _context.Customers
                .Where(c => c.Name.Contains(name))
                .OrderBy(c => c.Name)
                .ToListAsync();
        }
    }
}