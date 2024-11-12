using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ProductsApi
{
    public class ProductsContext : DbContext
     {
          public ProductsContext(DbContextOptions<ProductsContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
            
        }
    }
}