using Microsoft.EntityFrameworkCore;
using OnlineShoppingApp.Business.Operations.Order.Dtos;
using OnlineShoppingApp.Business.Types;
using OnlineShoppingApp.Data.Entities;
using OnlineShoppingApp.Data.Repositories;
using OnlineShoppingApp.Data.UnitOfWork;

namespace OnlineShoppingApp.Business.Operations.Order;

public class OrderManager : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<OrderEntity> _orderRepository;
    private readonly IRepository<OrderProductEntity> _orderProductRepository;
    private readonly IRepository<ProductEntity> _productRepository;

    public OrderManager(IRepository<OrderProductEntity> orderProductRepository,
        IRepository<OrderEntity> orderRepository, IUnitOfWork unitOfWork, IRepository<ProductEntity> productRepository)
    {
        _orderProductRepository = orderProductRepository;
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
    }

    public async Task<ServiceMessage> AddOrder(AddOrderDto order)
    {
        // Check input
        if (order == null || order.Products == null || !order.Products.Any())
        {
            return new ServiceMessage
            {
                IsSucceed = false,
                Message = "Order or products list cannot be empty"
            };
        }
        
        if (order.CustomerId <= 0)
        {
            return new ServiceMessage
            {
                IsSucceed = false,
                Message = "CustomerId is not valid"
            };
        }
        
        await _unitOfWork.BeginTransaction();

        var productIds = order.Products.Select(p => p.ProductId).ToList();
        
        // Fetch products
        var products = await _productRepository.GetAll()
            .Where(p => productIds.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id);

        // Stock and existence check
        foreach (var item in order.Products)
        {
            if(!products.TryGetValue(item.ProductId, out var product))
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = $"Product with ID {item.ProductId} not found"
                };

            if (product.StockQuantity < item.Quantity)
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = $"{product.ProductName} is out of stock (Stock: {product.StockQuantity})"
                };
        }
        
        decimal totalAmount = order.Products
            .Sum(item => products[item.ProductId].Price * item.Quantity);


        var orderEntity = new OrderEntity
        {
            CustomerId = order.CustomerId,
            OrderDate = DateTime.Now,
            TotalAmount = totalAmount,
        };
        
        _orderRepository.Add(orderEntity);

        try
        {
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception)
        {   
            await _unitOfWork.RollbackTransaction();
            throw new Exception("Order could not be created");
        }

        // Add products to order
        foreach (var item in order.Products)
        {
            var orderProduct = new OrderProductEntity
            {
                OrderId = orderEntity.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = products[item.ProductId].Price,
            };
            
            _orderProductRepository.Add(orderProduct);
            
            products[item.ProductId].StockQuantity -= item.Quantity;
            _productRepository.Update(products[item.ProductId]);
        }
        
        try
        {
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransaction();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransaction();
            
            var detailedMessage = ex.InnerException != null
                ? ex.InnerException.Message
                : ex.Message;
            throw new Exception("Order products could not be added. Rollback transaction\n" + detailedMessage, ex );
        }

        return new ServiceMessage
        {
            Message = "Order added successfully",
            IsSucceed = true
        };
    }

    public async Task<OrderDto> GetOrder(int id)
    {
        var order = await _orderRepository.GetAll(x => x.Id == id)
            .Select(x => new OrderDto
            {
                CustomerFullName = x.Customer.FirstName + " " + x.Customer.LastName,
                Id = x.Id,
                TotalAmount = x.TotalAmount,
                OrderDate = x.OrderDate,

                Products = x.OrderProducts.Select(p => new OrderProductDto
                {
                    Id = p.ProductId,
                    ProductName = p.Product.ProductName,
                    Quantity = p.Quantity
                }).ToList()
            }).FirstOrDefaultAsync();

        return order;
    }

    public async Task<List<OrderDto>> GetOrders()
    {
        var orders = await _orderRepository.GetAll()
            .Select(x => new OrderDto
            {
                CustomerFullName = x.Customer.FirstName + " " + x.Customer.LastName,
                Id = x.Id,
                TotalAmount = x.TotalAmount,
                OrderDate = x.OrderDate,

                Products = x.OrderProducts.Select(p => new OrderProductDto
                {
                    Id = p.ProductId,
                    ProductName = p.Product.ProductName,
                    Quantity = p.Quantity
                }).ToList()
            }).ToListAsync();

        return orders;
    }

    public async Task<ServiceMessage> Discount(int id, decimal discountPercentage)
    {
        if (discountPercentage < 0 || discountPercentage > 100)
            throw new Exception("Discount percentage must be between 0 and 100");

        var order = _orderRepository.GetById(id);

        if (order is null)
        {
            return new ServiceMessage
            {
                Message = "Order could not be found",
                IsSucceed = false
            };
        }

        order.TotalAmount = order.TotalAmount - (order.TotalAmount * (discountPercentage / 100));

        _orderRepository.Update(order);

        try
        {
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw new Exception("Error while discount process");
        }

        return new ServiceMessage
        {
            Message = "Discount successfully applied",
            IsSucceed = true
        };
    }

    public async Task<ServiceMessage> DeleteOrder(int id)
    {
        var order = _orderRepository.GetById(id);

        if (order is null)
        {
            return new ServiceMessage
            {
                Message = "Order could not be found",
                IsSucceed = false
            };
        }

        _orderRepository.Delete(id);

        try
        {
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw new Exception("Error while delete order process");
        }

        return new ServiceMessage
        {
            Message = "Order deleted",
            IsSucceed = true
        };
    }

    public async Task<ServiceMessage> UpdateOrder(UpdateOrderDto order)
    {
        var orderEntity = _orderRepository.GetAll()
            .Include(x => x.OrderProducts)
            .FirstOrDefault(x => x.Id == order.Id);

        if (orderEntity is null)
        {
            return new ServiceMessage
            {
                Message = "Order could not be found",
                IsSucceed = false
            };
        }

        await _unitOfWork.BeginTransaction();

        try
        {
            // Order bilgilerini güncelle
            orderEntity.OrderDate = order.OrderDate;
            orderEntity.TotalAmount = order.TotalAmount;
            orderEntity.CustomerId = order.CustomerId;

            // Mevcut ürünleri sil
            foreach (var existingProduct in orderEntity.OrderProducts.ToList())
            {
                _orderProductRepository.Delete(existingProduct, false);
            }

            // Yeni ürünleri ekle
            foreach (var productId in order.ProductIds)
            {
                var orderProduct = new OrderProductEntity
                {
                    ProductId = productId,
                    OrderId = order.Id,
                    Quantity = 1 // Varsayılan miktar veya DTO'dan alınabilir
                };
            
                _orderProductRepository.Add(orderProduct);
            }

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransaction();

            return new ServiceMessage
            {
                Message = "Order updated",
                IsSucceed = true
            };
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransaction();
            throw new Exception($"Error while updating order: {ex.Message}");
        }
    }
}