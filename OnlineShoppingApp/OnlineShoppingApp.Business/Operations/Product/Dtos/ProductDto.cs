namespace OnlineShoppingApp.Business.Operations.Product.Dtos;

public class ProductDto
{
    public int Id { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
}