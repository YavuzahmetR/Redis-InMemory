using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using StackExchangeApi.Web.Services;

namespace StackExchangeApi.Web.Controllers
{
    public class BaseController : Controller
    {
        private readonly RedisService _redisService;
        protected IDatabaseAsync databaseAsync;
        public BaseController(RedisService redisService)
        {
            _redisService = redisService;
            databaseAsync = redisService.GetDatabase(4);
        }
    
    }
}
