using OnlineShoppingApp.Business.Operations.Order.Dtos;
using OnlineShoppingApp.Business.Types;

namespace OnlineShoppingApp.Business.Operations.Order;

public interface IOrderService
{
    Task<ServiceMessage> AddOrder(AddOrderDto dto);
    Task<OrderDto>  GetOrder(int id);
    Task<List<OrderDto>> GetOrders();
    Task<ServiceMessage> Discount(int id, decimal discountPercentage);
    Task<ServiceMessage> DeleteOrder(int id);
    Task<ServiceMessage> UpdateOrder(UpdateOrderDto order);
    
    
}