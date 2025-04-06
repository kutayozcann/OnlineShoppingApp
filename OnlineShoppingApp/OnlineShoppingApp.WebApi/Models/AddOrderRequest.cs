using System.ComponentModel.DataAnnotations;

namespace OnlineShoppingApp.WebApi.Models;

public class AddOrderRequest
{
   public List<OrderProductItemRequest> Products { get; set; }
}

public class OrderProductItemRequest
{
   public int ProductId { get; set; }
   public int Quantity { get; set; } = 1; // Default
}