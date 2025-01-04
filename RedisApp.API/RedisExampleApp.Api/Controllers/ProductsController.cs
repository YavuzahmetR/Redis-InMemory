using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedisConnect;
using RedisExampleApp.Api.Models;
using RedisExampleApp.Api.Repositories;
using RedisExampleApp.Api.Services;
using StackExchange.Redis;

namespace RedisExampleApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService productService;
        //private readonly IDatabaseAsync _database;
        public ProductsController(IProductService productService)
        {
           this.productService = productService;
            //_database = database;
            //_database.StringSetAsync("test", "test");
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await productService.GetProductsAsync();
            return Ok(products);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            var createdProduct = await productService.CreateProductAsync(product);
            return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, createdProduct);
        }

    }
}
