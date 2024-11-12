using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ProductsApi
{
    public class ProductRepository : IProductRepository
   {
    private readonly ProductsContext _dbContext;

    public ProductRepository(ProductsContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Product>> GetAllProductsAsync()
    {
        return await _dbContext.Products.ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _dbContext.Products.FindAsync(id);
    }
}
}