using MovieRentalApp.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace MovieRentalApp.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://localhost:44321"; // Ajuste para sua porta

        public ApiService()
        {
            _httpClient = new HttpClient();
            _baseUrl = "https://localhost:44321";
            _httpClient.BaseAddress = new Uri(_baseUrl);
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        // 📋 CUSTOMERS
        public async Task<List<Customer>> GetCustomersAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<Customer>>("/customer") ?? new List<Customer>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar clientes: {ex.Message}");
            }
        }

        public async Task<Customer> CreateCustomerAsync(CreateCustomerRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/customer", request);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Customer>() ?? throw new Exception("Falha ao criar cliente");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao criar cliente: {ex.Message}");
            }
        }

        // 🎬 MOVIES
        public async Task<List<Movie>> GetMoviesAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<Movie>>("/movies") ?? new List<Movie>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar filmes: {ex.Message}");
            }
        }

        public async Task<Movie> CreateMovieAsync(CreateMovieRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/movies", request);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Movie>() ?? throw new Exception("Falha ao criar filme");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao criar filme: {ex.Message}");
            }
        }

        // 💳 RENTALS
        public async Task<Rental> CreateRentalAsync(CreateRentalRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/rental", request);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Rental>() ?? throw new Exception("Falha ao criar aluguel");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao criar aluguel: {ex.Message}");
            }
        }

        public async Task<List<Rental>> GetRentalsByCustomerAsync(string customerName)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<Rental>>($"/rental/customer/{customerName}") ?? new List<Rental>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar aluguéis: {ex.Message}");
            }
        }
    }
}