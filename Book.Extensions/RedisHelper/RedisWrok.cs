using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Extensions.RedisHelper
{
    public class RedisWrok:IRedisWrok
    {
        private readonly IDatabase db;
        private readonly RedisDB redisDb;
        private readonly ISubscriber sub;
        private readonly ILogger<RedisWrok> _logger;
        private readonly SerializeHelper _serializeHelper;

        public RedisWrok(RedisDB _redis, ILogger<RedisWrok> logger, SerializeHelper serializeHelper)
        {
            redisDb = _redis;
            db = redisDb.redisdb;
            sub = redisDb.Subscribe();
            _logger = logger;
            _serializeHelper = serializeHelper;
        }

        public List<T> getList<T>(string key)
        {
            throw new NotImplementedException();
        }

        public void ListRemove<T>(string key, T value)
        {
            throw new NotImplementedException();
        }

        public Task PublishAsync(string channels, string message)
        {
            return sub.PublishAsync(channels, message);
        }

        public Task<RedisValue> strGet(string key)
        {
            return db.StringGetAsync(key);
        }

        public Task strSet(string key, string value)
        {
            return db.StringSetAsync(key, value);
        }

        public Task SubScribeAsync(string channels)
        {
            return sub.SubscribeAsync(channels, (channels, message) =>
            {
                //_logger.LogInformation($"订阅的管道是{channels},发布的消息是{message}");
                Console.WriteLine($"订阅的管道是{channels},发布的消息是{message}");
            });
        }

        public async Task<bool> StringSetList<T>(string key, List<T> entitys)
        {
            if (entitys == null)
                throw new ArgumentNullException("参数为空");
            if (await db.KeyExistsAsync(key))
                return false;
            var valuestr = _serializeHelper.Serialize(entitys);
            return await db.StringSetAsync(key, valuestr);
        }

        public async Task<bool> HashSet<T>(string key, string filed, T t)
        {
            if (t == null)
                throw new ArgumentNullException("参数为空");
            if (await db.HashExistsAsync(key, filed))
                return false;
            var str = _serializeHelper.Serialize(t);
            return await db.HashSetAsync(key, filed, str);
        }

        public async Task<T> HashGet<T>(string key, string filed)
        {
            if (!await db.HashExistsAsync(key, filed))
                return default;
            string value = await db.HashGetAsync(key, filed);
            return _serializeHelper.DesSerialize<T>(value);
        }

        public async Task<List<T>> HashGetList<T>(string key)
        {
            if (!await db.KeyExistsAsync(key))
                return new List<T> { default };
            List<T> entitys = new();
            foreach (var value in await db.HashValuesAsync(key))
            {
                entitys.Add(_serializeHelper.DesSerialize<T>(value));
            }
            return entitys;
        }

        public async Task<long> Lpush(string key, string value)
        {
            if (!await db.KeyExistsAsync(key))
                return 0;
            return await db.ListLeftPushAsync(key, value);
        }
        public async Task<long> Rpush(string key, string value)
        {
            if (!await db.KeyExistsAsync(key)) return 0;
            return await db.ListRightPushAsync(key, value);
        }
        public async Task<List<T>> Range<T>(string key, long start, long stop)
        {
            if (!await db.KeyExistsAsync(key))
                return null;
            RedisValue[] values = await db.ListRangeAsync(key, start, stop);
            List<T> result = new List<T>();
            foreach (var value in values)
            {
                result.Add(_serializeHelper.DesSerialize<T>(value));
            }
            return result;
        }
        public int GetdatabaseIndex()
        {
            return db.Database;
        }
    }

    public static class RedisSetup
    {
        public static void AddRedisSetup(this IServiceCollection service)
        {
            service.AddScoped<RedisDB>();
            service.AddScoped<IRedisWrok,RedisWrok>();
        }
    }
}
