using System.ComponentModel.DataAnnotations;

namespace OnlineShoppingApp.WebApi.Models;

public class AddProductRequest
{
    [Required] [MaxLength(80)] public string ProductName { get; set; }
    [Required] public decimal Price { get; set; }
    [Required] public int StockQuantity { get; set; }
}