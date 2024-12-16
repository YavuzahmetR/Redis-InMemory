using Microsoft.AspNetCore.Mvc;
using StackExchangeApi.Web.Services;

namespace StackExchangeApi.Web.Controllers
{
    public class HashTypeController : BaseController
    {
        private readonly string hashKey = "dictionary";
        public HashTypeController(RedisService redisService) : base(redisService)
        {
        }
        public async Task<IActionResult> Index()
        {
            Dictionary<string, string> itemsDictionary = new Dictionary<string, string>();
            if(await databaseAsync.KeyExistsAsync(hashKey))
            {
                var items = await databaseAsync.HashGetAllAsync(hashKey);
                foreach (var item in items)
                {
                    itemsDictionary.Add(item.Name!, item.Value!);
                }
            }
            return View(itemsDictionary);
        }
    
    
        public async Task<IActionResult> AddItem(string field, string value)
        {
            await databaseAsync.HashSetAsync(hashKey, field, value);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteItem(string field)
        {
            await databaseAsync.HashDeleteAsync(hashKey, field);
            return RedirectToAction(nameof(Index));
        }
    }
}
