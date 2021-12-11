using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Extensions.RedisHelper
{
    public interface IRedisWrok
    {
        Task<RedisValue> strGet(string key);
        Task strSet(string key, string value);
        /// <summary>
        /// 移除listid内部的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void ListRemove<T>(string key, T value);
        /// <summary>
        /// 获取指定key的list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        List<T> getList<T>(string key);

        //订阅
        Task SubScribeAsync(string channels);
        Task PublishAsync(string channels, string message);

        Task<bool> StringSetList<T>(string key, List<T> entitys);
        Task<bool> HashSet<T>(string key, string filed, T t);
        Task<T> HashGet<T>(string key, string filed);
        Task<List<T>> HashGetList<T>(string key);
        int GetdatabaseIndex();
    }
}
