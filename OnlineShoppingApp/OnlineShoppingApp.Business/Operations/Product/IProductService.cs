using OnlineShoppingApp.Business.Operations.Product.Dtos;
using OnlineShoppingApp.Business.Types;

namespace OnlineShoppingApp.Business.Operations.Product;

public interface IProductService
{
    Task<ServiceMessage> AddProduct(AddProductDto product);
    Task<List<ProductDto>> GetProducts();
}