using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Book.API.Message
{
    public static class RedisCacheExtensions
    {
        public static DistributedCacheEntryOptions CreateOpions(int abloutions,int pow) { 
         DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
            var x = new Random().NextDouble();
            var ab = abloutions * pow * x + abloutions*(1 - x);
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(ab);
            options.SlidingExpiration=TimeSpan.FromSeconds(abloutions);
            return options;
        }

        public static async Task<TResult> GetOrCreateAsync<TResult>(this IDistributedCache _cache, string key,
            Func<DistributedCacheEntryOptions, Task<TResult>> valueFacotry, int ab, int pow) { 
          string value=await _cache.GetStringAsync(key);
            if (string.IsNullOrEmpty(value)) {
                var options=CreateOpions(ab, pow);
                var setValue = await valueFacotry(options);
                var strValue = JsonSerializer.Serialize(setValue);
                await _cache.SetStringAsync(key, strValue,options);
                return JsonSerializer.Deserialize<TResult>(await _cache.GetStringAsync(key));
            }
            await _cache.RefreshAsync(key);
            return JsonSerializer.Deserialize<TResult>(value);
        }
    }
}
