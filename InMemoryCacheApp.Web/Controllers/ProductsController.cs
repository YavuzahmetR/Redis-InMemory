using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryCacheApp.Web.Controllers
{
    public class ProductsController(IMemoryCache memoryCache) : Controller
    {
        public IActionResult Index()
        {
            MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions();

            cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(1);

            cacheEntryOptions.SlidingExpiration = TimeSpan.FromSeconds(10);
            cacheEntryOptions.Priority = CacheItemPriority.High;
            cacheEntryOptions.Priority = CacheItemPriority.Normal;
            cacheEntryOptions.Priority = CacheItemPriority.Low; //deletes the caches if memory is full from low to high priority

            cacheEntryOptions.Priority = CacheItemPriority.NeverRemove; //never deletes the caches with this priority 
            memoryCache.Set("CurrentTime", DateTime.Now.ToString(), cacheEntryOptions);

            return View();
        }

        public IActionResult Show()
        {
            memoryCache.TryGetValue("CurrentTime", out string currentTime);
            ViewBag.CurrentTime = currentTime;
            return View();
        }
    }
}
