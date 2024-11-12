using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace ProductsApi
{
    public class Product
    {
    [Key]
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
   
    public string Description { get; set; } = string.Empty;
   [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }
    }
}