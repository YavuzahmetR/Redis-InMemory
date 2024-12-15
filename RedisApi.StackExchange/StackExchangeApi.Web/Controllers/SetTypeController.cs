using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using StackExchangeApi.Web.Services;
using System.Xml.Linq;

namespace StackExchangeApi.Web.Controllers
{
    public class SetTypeController : Controller
    {
        private readonly IDatabaseAsync _dataBaseAsync;
        private readonly RedisService _redisService;
        private readonly string _setListKey = "hashnames";
        public SetTypeController(RedisService redisService)
        {
            _redisService = redisService;
            _dataBaseAsync = redisService.GetDatabase(2);
        }
        public async Task<IActionResult> Index()
        {
            HashSet<string> namesSet = new HashSet<string>();
            if (await _dataBaseAsync.KeyExistsAsync(_setListKey))
            {
                var names = await _dataBaseAsync.SetMembersAsync(_setListKey);
                foreach (var name in names)
                {
                    namesSet.Add(name.ToString());
                }
            }
           


            return View(namesSet);
        }

        [HttpPost]
        public async Task<IActionResult> AddItem(string name)
        {
            
            await _dataBaseAsync.SetAddAsync(_setListKey, name);
            #region AbsoluteExpiration
            //if (await _dataBaseAsync.KeyExistsAsync(_setListKey))
            //{
            //    await _dataBaseAsync.KeyExpireAsync(_setListKey, TimeSpan.FromMinutes(5));
            //}
            #endregion
            #region SlidingExpiration

            await _dataBaseAsync.KeyExpireAsync(_setListKey, TimeSpan.FromMinutes(2));

            #endregion

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteItem(string name)
        {
            await _dataBaseAsync.SetRemoveAsync(_setListKey, name);
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> DeleteRandom()
        {
            await _dataBaseAsync.SetPopAsync(_setListKey);
            return RedirectToAction(nameof(Index));
        }
    }
}
