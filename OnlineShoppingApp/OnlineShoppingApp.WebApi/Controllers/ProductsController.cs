using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineShoppingApp.Business.Operations.Product;
using OnlineShoppingApp.Business.Operations.Product.Dtos;
using OnlineShoppingApp.WebApi.Models;

namespace OnlineShoppingApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productService.GetProducts();
            return Ok(products);
        }
        
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProduct(AddProductRequest request)
        {
            var addProductDto = new AddProductDto
            {
                ProductName = request.ProductName,
                Price = request.Price,
                StockQuantity = request.StockQuantity
            };

            var result = await _productService.AddProduct(addProductDto);

            if (result.IsSucceed)
                return Ok(result);
            
            return BadRequest(result.Message);
        }
        
        
        
        
    }
}