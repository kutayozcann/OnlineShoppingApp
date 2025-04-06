using OnlineShoppingApp.Business.Operations.Product.Dtos;
using OnlineShoppingApp.Business.Types;
using OnlineShoppingApp.Data.Entities;
using OnlineShoppingApp.Data.Repositories;
using OnlineShoppingApp.Data.UnitOfWork;

namespace OnlineShoppingApp.Business.Operations.Product;

public class ProductManager : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<ProductEntity> _repository;

    public ProductManager(IUnitOfWork unitOfWork, IRepository<ProductEntity> repository)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
    }
    

    public async Task<ServiceMessage> AddProduct(AddProductDto product)
    {
        var hasProduct = _repository.GetAll(x => 
            x.ProductName.ToLower() == product.ProductName.ToLower()).Any();

        if (hasProduct)
        {
            return new ServiceMessage
            {
                IsSucceed = false,
                Message = $"Product {product.ProductName} already exists."
            };
        }

        var productEntity = new ProductEntity
        {
            ProductName = product.ProductName,
            Price = product.Price,
            StockQuantity = product.StockQuantity
        };
        
        _repository.Add(productEntity);

        try
        {
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception)
        {
            
            throw new Exception("Something went wrong while adding product.");
        }

        return new ServiceMessage
        {
            IsSucceed = true,
        };
    }

    public async Task<List<ProductDto>> GetProducts()
    {
        var products = _repository.GetAll()
            .Select(p => new ProductDto
            {
                Id = p.Id,
                ProductName = p.ProductName,
                Price = p.Price,
                StockQuantity = p.StockQuantity
            }).ToList();

        return products;
    }
}