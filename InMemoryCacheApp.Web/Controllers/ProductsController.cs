using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Mono.TextTemplating;
using Newtonsoft.Json.Linq;

namespace InMemoryCacheApp.Web.Controllers
{
    public class ProductsController(IMemoryCache memoryCache) : Controller
    {
        public IActionResult Index()
        {
            MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions();

            cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddSeconds(10);

            cacheEntryOptions.SlidingExpiration = TimeSpan.FromSeconds(10);
            cacheEntryOptions.Priority = CacheItemPriority.High;

            cacheEntryOptions.RegisterPostEvictionCallback((key, value, reason, state) =>
            {
                memoryCache.Set("RemovedKey", $"{key} - {value} - {reason} - {state}");
            });

            memoryCache.Set("CurrentTime", DateTime.Now.ToString(), cacheEntryOptions);

            return View();
        }

        public IActionResult Show()
        {
            memoryCache.TryGetValue("CurrentTime", out string currentTime);
            memoryCache.TryGetValue("RemovedKey", out string callback);
            ViewBag.CurrentTime = currentTime;
            ViewBag.Callback = callback;
            return View();
        }
    }
}
