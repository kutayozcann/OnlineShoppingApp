using OnlineShoppingApp.Data.Entities;

namespace OnlineShoppingApp.Business.Operations.Order.Dtos;

public class OrderDto
{
    public int Id { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime OrderDate { get; set; }
    public string CustomerFullName { get; set; }
    public List<OrderProductDto> Products { get; set; } 
}