using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryCacheApp.Web.Controllers
{
    public class ProductsController(IMemoryCache memoryCache) : Controller
    {
        public IActionResult Index()
        {
            if (!memoryCache.TryGetValue("run", out string time))// If the cache does not contain the key "run" assign it to "time" variable
             {
                memoryCache.Set<string>("run", DateTime.Now.ToString());// Set the key "run" to the current date and time create a cahce entry
            }
            time = memoryCache.Get<string>("run")!; //Get the value of the key "run" and assign it to the variable time

            ViewBag.run = time;// Assign the value of time to the ViewBag.run
            return View();
        }

        public IActionResult Show()
        {
            memoryCache.GetOrCreate<string>("zaman", entry => // Get the value of the key "zaman" or create a new cache entry(if it doesnt exists)
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10); // Config of cache entry
                return DateTime.Now.ToString(); // Return the current date and time, create a cache entry
            });
            memoryCache.Remove("run"); // Remove the key "run" from the cache
            ViewBag.time =  memoryCache.Get<string>("time"); // Get the value of the key "time" and assign it to the ViewBag.time
            return View();
        }
    }
}
