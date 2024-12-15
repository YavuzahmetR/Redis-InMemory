using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using StackExchangeApi.Web.Services;

namespace StackExchangeApi.Web.Controllers
{
    public class SortedSetTypeController : Controller
    {
        private readonly IDatabaseAsync databaseAsync;
        private readonly RedisService _redisService;
        private readonly string sortedSetKey = "sortedSetKey";
        public SortedSetTypeController(RedisService redisService)
        {
            _redisService = redisService;
            databaseAsync = redisService.GetDatabase(3);
        }
        public async Task<IActionResult> Index()
        {
            HashSet<string> namesSet = new HashSet<string>();
            #region ValueWithScore
            if (await databaseAsync.KeyExistsAsync(sortedSetKey))
            {
                await foreach (var name in databaseAsync.SortedSetScanAsync(sortedSetKey))
                {
                    namesSet.Add(name.ToString());
                }
            }
            #endregion
            #region ValueOnly
            if (await databaseAsync.KeyExistsAsync(sortedSetKey))
            {
                var names = await databaseAsync.SortedSetRangeByRankAsync(sortedSetKey);
                foreach (var name in names)
                {
                    namesSet.Add(name.ToString());
                }
            }
            #endregion
            return View(namesSet);
        }

        public async Task<IActionResult> AddItem(string name, int score)
        {
            await databaseAsync.SortedSetAddAsync(sortedSetKey,name,score);
            await databaseAsync.KeyExpireAsync(sortedSetKey, TimeSpan.FromMinutes(2));
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteItemG(string name)
        {
            await databaseAsync.SortedSetRemoveAsync(sortedSetKey, name);
            //SortedSetRangeByRankAsync works only with this method. Doesnt work with scores!
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteScoresBetween(double start, double stop)
        {
            await databaseAsync.SortedSetRemoveRangeByScoreAsync(sortedSetKey, start, stop);
            //works with every score bringer method.
            return RedirectToAction(nameof(Index));
        }
    }
}
