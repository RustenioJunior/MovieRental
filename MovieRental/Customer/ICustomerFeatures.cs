using MovieRental.DTOs.Customer;

namespace MovieRental.Customer
{
    public interface ICustomerFeatures
    {
        Task<Customer> CreateCustomerAsync(Customer customer);
        Task<Customer> GetCustomerByIdAsync(int id);
        Task<IEnumerable<Customer>> GetCustomersByNameAsync(string name);
        Task<Customer> GetCustomerByEmailAsync(string email);
    }
}