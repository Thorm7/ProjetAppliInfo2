using Npgsql;
using LiberNet.Services;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration
    .GetConnectionString("DefaultConnection");

builder.Services.AddScoped(_ =>
{
    var connection = new NpgsqlConnection(connectionString);
    connection.Open();
    return connection;
});

builder.Services.AddScoped<LivreService>();
builder.Services.AddScoped<CategorieService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<EmpruntService>();
builder.Services.AddRazorPages();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// Middleware : couche que chaque requête HTTP traverse
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();
app.MapRazorPages()
    .WithStaticAssets();

app.Run();