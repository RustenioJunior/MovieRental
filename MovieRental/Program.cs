using MovieRental.Customer;
using MovieRental.Data;
using MovieRental.Middlewares;
using MovieRental.Movie;
using MovieRental.PaymentProviders;
using MovieRental.Rental;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConsole();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddEntityFrameworkSqlite().AddDbContext<MovieRentalDbContext>();
builder.Services.AddEntityFrameworkSqlite().AddDbContext<MovieRentalDbContext>();

// ✅ REGISTRAR PAYMENT PROVIDERS
builder.Services.AddScoped<MbWayProvider>();
builder.Services.AddScoped<PayPalProvider>();
builder.Services.AddScoped<DebitCardProvider>();
builder.Services.AddScoped<CreditCardProvider>();
builder.Services.AddScoped<IPaymentProviderFactory, PaymentProviderFactory>();

// ✅ Registrar serviços
builder.Services.AddScoped<IRentalFeatures, RentalFeatures>();
builder.Services.AddScoped<ICustomerFeatures, CustomerFeatures>();
builder.Services.AddScoped<IMovieFeatures, MovieFeatures>();

var app = builder.Build();

app.UseGlobalExceptionHandler();

// environment handling
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

using (var client = new MovieRentalDbContext())
{
    client.Database.EnsureCreated();
}
// checkdb
app.MapGet("/check-db", (MovieRentalDbContext context) =>
{
    var canConnect = context.Database.CanConnect();
    var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "movierental.db");

    return Results.Ok(new
    {
        databaseExists = File.Exists(dbPath),
        canConnect = canConnect,
        dbPath = dbPath
    });
});

app.Run();