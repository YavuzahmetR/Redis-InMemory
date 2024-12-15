using IDistributedCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;

namespace IDistributedCacheRedisApp.Web.Controllers
{
    public class ProductsController(IDistributedCache distributedCache) : Controller
    {
        public async Task<IActionResult> Index()
        {
            Product product = new()
            {
                Id = 1,
                Name = "Laptop",
                Price = 1000
            };

            Product product2 = new()
            {
                Id = 2,
                Name = "Laptop2",
                Price = 2000
            };

            await distributedCache.SetAsync("product:1", Encoding.UTF8.GetBytes(JsonSerializer.Serialize(product)), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
            });

            await distributedCache.SetStringAsync("product:2", JsonSerializer.Serialize(product2), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
            });

            return View();
        }
        public async Task<IActionResult> Get()
        {
            var productObject = await distributedCache.GetAsync("product:1");
            var productObject2 = await distributedCache.GetStringAsync("product:2");

            Product product = JsonSerializer.Deserialize<Product>(productObject)!;
            Product product2 = JsonSerializer.Deserialize<Product>(productObject2)!;

            ViewBag.Product = product;
            ViewBag.Product2 = product2;

            return View();
        }

        public async Task<IActionResult> Remove()
        {
            await distributedCache.RemoveAsync("product:1");

            return View();
        }
    }
}
