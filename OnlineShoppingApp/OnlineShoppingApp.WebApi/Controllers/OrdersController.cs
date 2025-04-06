using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineShoppingApp.Business.Operations.Order;
using OnlineShoppingApp.Business.Operations.Order.Dtos;
using OnlineShoppingApp.WebApi.Filters;
using OnlineShoppingApp.WebApi.Models;

namespace OnlineShoppingApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await _orderService.GetOrder(id);

            if (order is null)
                return NotFound();

            return Ok(order);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _orderService.GetOrders();

            return Ok(orders);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> AddOrder(AddOrderRequest request)
        {
            if (request is null || request.Products is null)
            {
                return BadRequest("Requese body cannot be empty");
            }
            
            //Getting customerId from JWT
            var customerIdClaim = (User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (string.IsNullOrEmpty(customerIdClaim) || !int.TryParse(customerIdClaim, out int customerId))
            {
                return Unauthorized("Invalid user identity");
            }
            
            var addOrderDto = new AddOrderDto
            {
                CustomerId = customerId,
                Products = request.Products.Select(p => new OrderProductItemDto
                {
                    ProductId = p.ProductId,
                    Quantity = p.Quantity
                }).ToList()
            };

            var result = await _orderService.AddOrder(addOrderDto);

            if (!result.IsSucceed)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        [HttpPatch("{id}/discount")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Discount(int id, decimal discountPercentage)
        {
            var result = await _orderService.Discount(id, discountPercentage);

            if (!result.IsSucceed)
                return NotFound(result.Message);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var result = await _orderService.DeleteOrder(id);
            

            if (!result.IsSucceed)
                return NotFound(result.Message);
            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [TimeControlFilter]
        public async Task<IActionResult> UpdateOrder(int id, UpdateOrderRequest request)
        {
            var updateOrderDto = new UpdateOrderDto
            {
                TotalAmount = request.TotalAmount,
                CustomerId = request.CustomerId,
                OrderDate = request.OrderDate,
                ProductIds = request.ProductIds,
                Id = id
            };

            var result = await _orderService.UpdateOrder(updateOrderDto);

            if (!result.IsSucceed)
                return NotFound(result.Message);
            return await GetOrder(id);
        }
    }
}