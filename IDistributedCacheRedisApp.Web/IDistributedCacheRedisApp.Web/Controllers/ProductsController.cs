using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;

namespace IDistributedCacheRedisApp.Web.Controllers
{
    public class ProductsController(IDistributedCache distributedCache) : Controller
    {
        public async Task<IActionResult> Index()
        {
            await distributedCache.SetAsync("name", Encoding.UTF8.GetBytes("John"), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1),
            });

            await distributedCache.SetStringAsync("age", "30", new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(1),
                SlidingExpiration = TimeSpan.FromSeconds(30)
            });
            return View();
        }
        public async Task<IActionResult> Get()
        {
            //var name = await distributedCache.GetStringAsync("name"); can be exuted like this also.

            var name = await distributedCache.GetAsync("name"); //returns byte[]

            var nameBytes = name != null ? Encoding.UTF8.GetString(name) : null;

            var age = await distributedCache.GetStringAsync("age"); //returns string

            ViewBag.Name = nameBytes;
            ViewBag.Age = age;

            return View();
        }

        public async Task<IActionResult> Remove()
        {
            await distributedCache.RemoveAsync("name");
            await distributedCache.RemoveAsync("age");
            return View();
        }
    }
}
