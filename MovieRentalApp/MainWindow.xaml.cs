using MovieRentalApp.Models;
using MovieRentalApp.Services;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MovieRentalApp
{
    public partial class MainWindow : Window
    {
        private readonly ApiService _apiService;

        public MainWindow()
        {
            InitializeComponent();
            _apiService = new ApiService();
            _ = LoadComboBoxes(); // ✅ Usar discard (_) para chamada async no construtor
        }

        private async Task LoadComboBoxes()
        {
            try
            {
                // Carregar combos na inicialização
                var customers = await _apiService.GetCustomersAsync();
                var movies = await _apiService.GetMoviesAsync();

                cmbCustomers.ItemsSource = customers;
                cmbMovies.ItemsSource = movies;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar dados: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // 📋 CLIENTES - CORRIGIDOS
        private async Task LoadCustomers_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var customers = await _apiService.GetCustomersAsync();
                lstCustomers.ItemsSource = customers;

                // Atualizar combo também
                cmbCustomers.ItemsSource = customers;

                MessageBox.Show($"Carregados {customers.Count} clientes", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar clientes: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void CreateCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCustomerName.Text))
            {
                MessageBox.Show("Nome é obrigatório", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var request = new CreateCustomerRequest
                {
                    Name = txtCustomerName.Text.Trim(),
                    Email = txtCustomerEmail.Text.Trim(),
                    Phone = txtCustomerPhone.Text.Trim()
                };

                var customer = await _apiService.CreateCustomerAsync(request);

                MessageBox.Show($"Cliente '{customer.Name}' criado com ID: {customer.Id}", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

                // Limpar campos
                txtCustomerName.Text = txtCustomerEmail.Text = txtCustomerPhone.Text = "";

                // ✅ AGORA FUNCIONA - await com Task
                await LoadCustomers_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao criar cliente: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // 🎬 FILMES - CORRIGIDOS
        private async Task LoadMovies_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var movies = await _apiService.GetMoviesAsync();
                lstMovies.ItemsSource = movies;

                // Atualizar combo também
                cmbMovies.ItemsSource = movies;

                MessageBox.Show($"Carregados {movies.Count} filmes", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar filmes: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Validação para permitir apenas números no ano
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private async void CreateMovie_Click(object sender, RoutedEventArgs e)
        {
            // Validações dos novos campos
            if (string.IsNullOrWhiteSpace(txtMovieTitle.Text))
            {
                MessageBox.Show("Título é obrigatório", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (cmbMovieGenre.SelectedItem == null)
            {
                MessageBox.Show("Gênero é obrigatório", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtMovieDescription.Text))
            {
                MessageBox.Show("Descrição é obrigatória", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(txtMovieReleaseYear.Text, out int releaseYear) || releaseYear < 1900 || releaseYear > 2100)
            {
                MessageBox.Show("Ano de lançamento deve ser entre 1900 e 2100", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var request = new CreateMovieRequest
                {
                    Title = txtMovieTitle.Text.Trim(),
                    Genre = (cmbMovieGenre.SelectedItem as ComboBoxItem)?.Content.ToString(),
                    Description = txtMovieDescription.Text.Trim(),
                    ReleaseYear = releaseYear
                    // Adicione outros campos se necessário: Director, Price, etc.
                };

                var movie = await _apiService.CreateMovieAsync(request);

                MessageBox.Show($"Filme '{movie.Title}' criado com ID: {movie.Id}", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

                // Limpar todos os campos
                ClearMovieForm();

                // ✅ AGORA FUNCIONA
                await LoadMovies_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao criar filme: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Método para limpar o formulário (adicione este método)
        private void ClearMovieForm()
        {
            txtMovieTitle.Text = "";
            cmbMovieGenre.SelectedIndex = -1;
            txtMovieReleaseYear.Text = DateTime.Now.Year.ToString();
            txtMovieDescription.Text = "";
        }

        // 💳 ALUGUÉIS - CORRIGIDOS
        private async void CreateRental_Click(object sender, RoutedEventArgs e)
        {
            if (cmbCustomers.SelectedItem == null || cmbMovies.SelectedItem == null)
            {
                MessageBox.Show("Selecione um cliente e um filme", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(txtRentalDays.Text, out int days) || days <= 0)
            {
                MessageBox.Show("Dias deve ser um número positivo", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var customer = (Customer)cmbCustomers.SelectedItem;
                var movie = (Movie)cmbMovies.SelectedItem;
                var paymentMethod = ((ComboBoxItem)cmbPaymentMethod.SelectedItem).Content.ToString().ToLower();

                var request = new CreateRentalRequest
                {
                    CustomerId = customer.Id,
                    MovieId = movie.Id,
                    DaysRented = days,
                    PaymentMethod = paymentMethod
                };

                var rental = await _apiService.CreateRentalAsync(request);

                MessageBox.Show($"Aluguel criado com sucesso!\nTransaction: {rental.Id}\nDevolução: {rental.ExpectedReturnDate:dd/MM/yyyy}",
                    "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

                // Limpar seleção
                txtRentalDays.Text = "3";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao criar aluguel: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void SearchRentals_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearchCustomer.Text))
            {
                MessageBox.Show("Digite um nome para buscar", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var rentals = await _apiService.GetRentalsByCustomerAsync(txtSearchCustomer.Text.Trim());
                lstRentals.ItemsSource = rentals;

                MessageBox.Show($"Encontrados {rentals.Count} aluguéis", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao buscar aluguéis: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ✅ MÉTODOS PARA OS BOTÕES (precisam ser void para eventos WPF)
        private async void LoadCustomersButton_Click(object sender, RoutedEventArgs e)
        {
            await LoadCustomers_Click(sender, e);
        }

        private async void LoadMoviesButton_Click(object sender, RoutedEventArgs e)
        {
            await LoadMovies_Click(sender, e);
        }
    }
}