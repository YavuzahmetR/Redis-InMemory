﻿using StackExchange.Redis;

namespace StackExchange.Api.Services
{
    public class RedisService
    {
        private ConnectionMultiplexer _redis;
        private readonly string _redisPort;
        private readonly string _redisHost;

        public IDatabaseAsync database { get; set; }
        public RedisService(IConfiguration configuration)
        {
            _redisHost = configuration["Redis:Host"]!;
            _redisPort = configuration["Redis:Port"]!;
        }

        public async Task ConnectAsync()
        {
            var configString = $"{_redisHost}:{_redisPort}";
            await ConnectionMultiplexer.ConnectAsync(configString);
            
        }   

        public IDatabaseAsync GetDatabase(int db)
        {
            return _redis.GetDatabase(db);
        }
    }
    
}