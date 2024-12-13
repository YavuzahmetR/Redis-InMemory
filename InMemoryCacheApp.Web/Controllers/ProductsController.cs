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
            /* Access at exactly 51 seconds:
                SlidingExpiration comes into play and extends the cache's duration by 10 seconds from the last access.
                In this case, the cache is extended until the 61st second (1 minute and 1 second).
                1 minute (60th second) limit:
                However, since the AbsoluteExpiration limit will come into effect at the 60th second, the cache cannot exceed this limit and is deleted at the 60th second.*/

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
