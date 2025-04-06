namespace OnlineShoppingApp.Business.Operations.Product.Dtos;

public class AddProductDto
{
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
}