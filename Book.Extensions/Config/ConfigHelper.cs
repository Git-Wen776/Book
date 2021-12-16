using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Extensions.Config
{
   public  class ConfigHelper
    {
        private readonly IConfiguration Config;
        
        public ConfigHelper(IConfiguration _config){
            Config = _config;
        }

        public ConfigHelper() {
            string path = "appsetting.json";
            Config = new ConfigurationBuilder().
                SetBasePath(@"E:\vs 2019\BookStore\Book.API\").
                Add(new JsonConfigurationSource
                {
                    Path = path,
                    Optional = false,
                    ReloadOnChange = true
                }).Build();
        }

        public  string settingStr(params string[] settings) {
            var section = string.Join(':', settings);
            if (settings is not null) {
                return Config.GetSection(section).Value;
            }
            return null;
        }

        public  List<T> GetValues<T>(string[] param)
        {
            List<T> list = new List<T>();
            Config.Bind(string.Join(':', param), list);
            return list;
        }

        public  T GetObject<T>(string[] param)
        {
            T t=default;
            Config.Bind(string.Join(':', param), t);
            return t;
        }
    }

}
