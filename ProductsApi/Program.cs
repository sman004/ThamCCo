using Microsoft.EntityFrameworkCore;
using ProductsApi;

var builder = WebApplication.CreateBuilder(args);

// Configure database context
builder.Services.AddDbContext<ProductsContext>(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        var dbPath = System.IO.Path.Join(path, "Products.db");
        options.UseSqlite($"Data Source={dbPath}");
        options.EnableDetailedErrors();
        options.EnableSensitiveDataLogging();
    }
    else
    {
        var cs = builder.Configuration.GetConnectionString("ProductsContext");
        options.UseSqlServer(cs); // Azure SQL for production
    }
});

// Register the repository
if (builder.Environment.IsDevelopment())
{
    // Use the fake repository in development
    builder.Services.AddScoped<IProductRepository, FakeProductRepository>();
}
else
{
    // Use the actual repository in production
    builder.Services.AddScoped<IProductRepository, ProductRepository>();
}

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

// API endpoints for product access
app.MapGet("/api/products", async (IProductRepository productRepo) =>
{
    try
    {
        var products = await productRepo.GetAllProductsAsync();
        return Results.Ok(products);
    }
    catch (Exception ex)
    {
        return Results.Problem("An error occurred while retrieving products: " + ex.Message);
    }
});

app.MapGet("/api/products/{id}", async (int id, IProductRepository productRepo) =>
{
    var product = await productRepo.GetProductByIdAsync(id);
    return product != null ? Results.Ok(product) : Results.NotFound();
});

// Test endpoint
app.MapGet("/", () => "Hiya!");

app.Run();
