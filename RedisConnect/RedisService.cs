using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisConnect
{
    public class RedisService 
    {
        private readonly ConnectionMultiplexer _connectionMultiplexer;
        public RedisService(string url)
        {
            _connectionMultiplexer = ConnectionMultiplexer.Connect(url);
        }

        public IDatabaseAsync GetDatabaseAsync(int db)
        {
            return _connectionMultiplexer.GetDatabase(db);
        }
    }
}
