using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Extensions.LogExcptions
{
    public static class NlogSetup
    {
        public static void AddLoggerSetup(this IServiceCollection service)
        {
            service.AddSingleton<ILoggerRecord, LoggerRecord>();
        }
    }
}
