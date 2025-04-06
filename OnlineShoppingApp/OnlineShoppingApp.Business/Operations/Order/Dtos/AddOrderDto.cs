namespace OnlineShoppingApp.Business.Operations.Order.Dtos;

public class AddOrderDto
{
    public int CustomerId { get; set; }
    public List<OrderProductItemDto> Products { get; set; }
}

public class OrderProductItemDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}