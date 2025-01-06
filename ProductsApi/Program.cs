using Microsoft.EntityFrameworkCore;
using ProductsApi;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

[assembly: InternalsVisibleTo("ProductsApi.Test")] // Expose the Program class to the test project

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient<ProductsApi.Services.ThirdPartyProductService>();

//add  authO authentication 
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Auth:Authority"];
        options.Audience =  builder.Configuration["Auth:Audience"];
    });
    builder.Services.AddAuthorization();


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
        // Azure SQL for production
    {
       var cs = builder.Configuration.GetConnectionString("ProductsContext");
        options.UseSqlServer(cs, sqlServerOptionsAction: sqlOptions =>
        {
             //add retry pattern for resilience 
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(6),
                errorNumbersToAdd: null);
        }); 
       
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


//consuming products api from third party suppliers and adding the products to database
app.MapPost("/sync-products", async (ProductsContext db, ProductsApi.Services.ThirdPartyProductService productService) =>
{
    string apiUrl = "http://undercutters.azurewebsites.net/api/product";
    try
    {
        var products = await productService.GetProductsFromApiAsync(apiUrl);
        db.Products.AddRange(products);
        await db.SaveChangesAsync();

        return Results.Ok("Products added successfully!");
    }
    catch (Exception ex)
    {
        return Results.Problem($"Failed to sync products: {ex.Message}");
    }
});


// API endpoints for product access

app.MapGet("/api/products",  [Authorize] async (IProductRepository productRepo) =>
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
