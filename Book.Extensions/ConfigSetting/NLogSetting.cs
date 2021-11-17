using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog.Web;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace Book.Extensions.ConfigSetting
{
   public static class NLogSetting
    {
        public static void AddNlog(this IServiceCollection services) {
            services.AddLogging(builder=> {
                builder.AddNLogWeb();
                builder.AddNLog();
            });
        }

    }


}
