using MovieRentalCustomer = MovieRental.Customer.Customer; // Alias to avoid confusion with other Customer classes
using MovieRental.DTOs.Customer;

namespace MovieRental.Customer
{
    public interface ICustomerFeatures
    {
        Task<MovieRentalCustomer> CreateCustomerAsync(MovieRentalCustomer customer);
        Task<MovieRentalCustomer> GetCustomerByIdAsync(int id);
        Task<IEnumerable<MovieRentalCustomer>> GetCustomersByNameAsync(string name);
        Task<MovieRentalCustomer> GetCustomerByEmailAsync(string email);
        Task<IEnumerable<MovieRentalCustomer>> GetAllCustomersAsync(); 
    }
}