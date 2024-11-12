using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsApi
{
    public class FakeProductRepository : IProductRepository
{
    public async Task<List<Product>> GetAllProductsAsync()
    {
        // Simulating a fake product list for testing
        return new List<Product>
        {
            new Product { ProductId = 1, ProductName = "Nike", Description = "Sandals", Price = 200.00M },
            new Product { ProductId = 2, ProductName = "Adidas", Description = "Sneakers", Price = 250.00M }
        };
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        // Simulating getting a fake product by ID
        return id == 1
            ? new Product { ProductId = 1, ProductName = "Nike", Description = "Sandals", Price = 200.00M }
            : null;
    }
}
}