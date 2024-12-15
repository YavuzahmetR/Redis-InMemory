using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using StackExchangeApi.Web.Services;

namespace StackExchangeApi.Web.Controllers
{
    public class ListTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabaseAsync dataBaseAsync;
        private readonly string ListKey = "listKey";
        public ListTypeController(RedisService redisService)
        {
            _redisService = redisService;
            dataBaseAsync = redisService.GetDatabase(1);
        }
        public async Task<IActionResult> Index()
        {
            List<string> namesList = new List<string>();
            if(await dataBaseAsync.KeyExistsAsync(ListKey))
            {
                var names = await dataBaseAsync.ListRangeAsync(ListKey);
                foreach (var name in names)
                {
                    namesList.Add(name.ToString());
                }
            }
            return View(namesList);
        }

        [HttpPost]
        public async Task<IActionResult> AddItem(string name)
        {
            await dataBaseAsync.ListRightPushAsync(ListKey, name);
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> DeleteItem(string name)
        {
            await dataBaseAsync.ListRemoveAsync(ListKey,name);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteFirstItem(string name)
        {
            await dataBaseAsync.ListLeftPopAsync(ListKey);
            return RedirectToAction("Index");
        }
    }
}
