using MovieRental.Customer;
using MovieRental.Data;
using MovieRental.Middlewares;
using MovieRental.Movie;
using MovieRental.PaymentProviders;
using MovieRental.Rental;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConsole(); // Simple Logging 

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddEntityFrameworkSqlite().AddDbContext<MovieRentalDbContext>();
builder.Services.AddTransient<GlobalExceptionMiddleware>();

// ✅ REGISTRAR PAYMENT PROVIDERS
builder.Services.AddScoped<MbWayProvider>();
builder.Services.AddScoped<PayPalProvider>();
builder.Services.AddScoped<DebitCardProvider>();
builder.Services.AddScoped<IPaymentProviderFactory, PaymentProviderFactory>();

// ✅ Registrar outros serviços existentes
builder.Services.AddScoped<IRentalFeatures, RentalFeatures>();
builder.Services.AddScoped<ICustomerFeatures, CustomerFeatures>();
builder.Services.AddScoped<IMovieFeatures, MovieFeatures>();

var app = builder.Build();

// environment handling
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage(); // Detailed stack traces for debugging
    app.UseGlobalExceptionHandler();
}
else
{
    app.UseExceptionHandler("/error"); // Clean responses in production
}

// Universal error path
app.Map("/error", () => Results.Problem(
    title: "Something went wrong",
    statusCode: StatusCodes.Status500InternalServerError
));

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

using (var client = new MovieRentalDbContext())
{
    client.Database.EnsureCreated();
}

app.Run();