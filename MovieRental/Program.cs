using MovieRental.Customer;
using MovieRental.Data;
using MovieRental.Movie;
using MovieRental.Rental;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConsole(); // Simple Logging 

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddEntityFrameworkSqlite().AddDbContext<MovieRentalDbContext>();

// Register services for dependency injection
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