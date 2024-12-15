using IDistributedCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace IDistributedCacheRedisApp.Web.Controllers
{
    public class ProductsController(IDistributedCache distributedCache) : Controller
    {
        public IActionResult Index()
        {

            return View();

        }
        public IActionResult Get()
        {

            return View();
        }
        public async Task<IActionResult> ImageUrl()
        {
            var bytes = await distributedCache.GetAsync("imageFile")!;
            return File(bytes!, "image/jpeg");
        }
        public async Task<IActionResult> ImageCache()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/askim.jpg");
            Byte[] imageFile = System.IO.File.ReadAllBytes(path);
            await distributedCache.SetAsync("imageFile", imageFile, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });
            return View();
        }
        public async Task<IActionResult> Remove()
        {
            await distributedCache.RemoveAsync("product:1");

            return View();
        }
    }
}
