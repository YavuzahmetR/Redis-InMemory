using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using StackExchangeApi.Web.Services;
using System.Xml.Linq;

namespace StackExchangeApi.Web.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabaseAsync dataBaseAsync;
        public StringTypeController(RedisService redisService)
        {
            _redisService = redisService;
            dataBaseAsync = redisService.GetDatabase(0);
        }
        public async Task<IActionResult> Index()
        {
            //var name = await dataBaseAsync.StringSetAndGetAsync("name", "John Doe"); &Set and get in one operation&
            //ViewBag.Name = name;

            await dataBaseAsync.StringSetAsync("name", "John Doe");
            await dataBaseAsync.StringSetAsync("hostCount", 100);
            return View();
        }
        public async Task<IActionResult> Show()
        {
            
            var name = await dataBaseAsync.StringGetAsync("name");
            if (name.HasValue)
            {
                ViewBag.Name = name.ToString();
            }
            var nameRange = await dataBaseAsync.StringGetRangeAsync("name", 0, 3);
            if (nameRange.HasValue)
            {
                ViewBag.NameRange = nameRange.ToString();
            }

            //var hostCount = await dataBaseAsync.StringGetAsync("hostCount"); &Get value as string&
            //await dataBaseAsync.StringIncrementAsync("hostCount", 15);
            
            var hostCount = await dataBaseAsync.StringIncrementAsync("hostCount",15);

            ViewBag.HostCount = hostCount;

           return View();
        }
        public IActionResult Remove()
        {
            return View();
        }
    }
}
