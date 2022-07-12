using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSRedis;
using Microsoft.Extensions.Logging;

namespace Book.Extensions.CsRedisExtensions
{
    public class CsRedisInstance
    {
        private readonly RedisClientPool pool;

        private readonly Config.ConfigHelper _config;

        private readonly RedisClient _redisClient;

        private readonly ILogger<CsRedisInstance> _logger;
        public CsRedisInstance(Config.ConfigHelper configHelper,ILogger<CsRedisInstance> logger)
        {
            _logger = logger;
            _config = configHelper;
            pool = new RedisClientPool(_config.settingStr(new string[] { "Csredis", "RedisConnect" }), o =>
            {
                o.Connected += (sender, e) =>
                  {
                      _logger.LogInformation($"CsRedis Connected Type Is {sender.GetType()}");
                      var type=sender.GetType();
                      if (type != typeof(string))
                      {
                          throw new RedisException("connection has error");
                      }
                  };
                o.ReceiveTimeout = 300;
                o.ReconnectWait = 300;
            });
            _redisClient = pool.Get().Value;
        }

        
    }
}
