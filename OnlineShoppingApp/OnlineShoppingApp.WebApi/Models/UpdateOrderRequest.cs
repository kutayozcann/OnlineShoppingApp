using System.ComponentModel.DataAnnotations;

namespace OnlineShoppingApp.WebApi.Models;

public class UpdateOrderRequest
{
    [Required]
    public decimal TotalAmount { get; set; }
    [Required]
    public int CustomerId { get; set; }
    [Required]
    public DateTime OrderDate { get; set; }
    [Required]
    public List<int>  ProductIds { get; set; }
}