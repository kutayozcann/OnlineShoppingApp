namespace OnlineShoppingApp.Business.Operations.Order.Dtos;

public class UpdateOrderDto
{
    public int Id { get; set; }
    public decimal TotalAmount { get; set; }
    public int CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public List<int> ProductIds { get; set; }
    
}