using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Extensions.RedisHelper
{
    public class SerializeHelper
    {
        private static object obj = 1;
        private static SerializeHelper serialize;
        public static SerializeHelper GetSerialize()
        {
            if (serialize == null)
            {
                lock (obj)
                {
                    if (serialize == null)
                        serialize = new SerializeHelper();
                }
            }
            return serialize;
        }

        public string Serialize<T>(List<T> entitys)
        {
            return JsonConvert.SerializeObject(entitys);
        }
        public string Serialize<T>(T t)
        {
            return JsonConvert.SerializeObject(t);
        }
        public T DesSerialize<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
    }

    public static  class SrializeSetup
    {
        public static void AddSerializeSetup(this IServiceCollection service)
        {
            service.AddScoped<SerializeHelper>(p=>SerializeHelper.GetSerialize());
        }
    }
}
