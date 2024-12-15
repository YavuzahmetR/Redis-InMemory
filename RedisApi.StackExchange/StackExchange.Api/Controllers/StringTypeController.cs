using Microsoft.AspNetCore.Mvc;
using StackExchange.Api.Services;
using StackExchange.Redis;

namespace StackExchange.Api.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly RedisService _redisService;
        private IDatabaseAsync dataBaseAsync;
        public StringTypeController(RedisService redisService)
        {
            _redisService = redisService;
            dataBaseAsync = redisService.GetDatabase(0);
        }
        public async Task<IActionResult> Index()
        {
            var name = await dataBaseAsync.StringSetAndGetAsync("name", "John Doe");
            ViewBag.Name = name;
            return View();
        }
        public IActionResult Show()
        {
            return View();
        }
        public IActionResult Remove()
        {
            return View();
        }
    }
}
