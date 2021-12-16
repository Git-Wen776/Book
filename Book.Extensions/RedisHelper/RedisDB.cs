using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Book.Extensions.Config;
using StackExchange.Redis;

namespace Book.Extensions.RedisHelper
{
    public class RedisDB
    {
        private ConnectionMultiplexer _redis;
        private readonly Object obj=new object();
        public  IDatabase redisdb { get { return _db; } }
        private readonly ConfigHelper _config;
        private IDatabase _db;
       
        public RedisDB(ConfigHelper config)
        {
            _config = config;
            var connectionstr = _config.settingStr(new string[] { "ConfigurationOptions", "host" });
            if (connectionstr == null)
                throw new ArgumentException("redis connectstring is empty");
            RedidConnect(connectionstr);
            _db = _redis.GetDatabase();
        }

        private void RedidConnect(string connect)
        {
            if (_redis != null && _redis.IsConnected)
                return;
            if(_redis == null)
            {
                lock (obj)
                {
                    if (_redis == null)
                    {
                       ConfigurationOptions options=new ConfigurationOptions()
                       {
                           DefaultDatabase = 1,
                           AbortOnConnectFail = false,
                           ConnectRetry = 4,
                           Password = "123456",
                           ConnectTimeout = 3,
                           EndPoints = {connect}
                       };
                       this. _redis=ConnectionMultiplexer.Connect(options);
                    }
                }
            }
        }

        public ISubscriber Subscribe()
        {
            return _redis.GetSubscriber();
        }
    }
}
