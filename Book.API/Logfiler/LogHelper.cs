using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.Extensions.Logging;
using NLog;

namespace Book.API.Logfiler
{
    public class LogHelper:ILogHelper
    {
        private readonly ILogger<LogHelper> logger;

        public LogHelper(ILogger<LogHelper> _logger) {
            logger = _logger;
        }

        public void Excpetion(Type source, Exception ex)
        {
            var log = GetLogger(source);
            if(log is not null)
            {
                
            }
        }


       public NLog.ILogger GetLogger(Type source)
        {
            return NLog.LogManager.GetCurrentClassLogger(source);
        }
    }
}
