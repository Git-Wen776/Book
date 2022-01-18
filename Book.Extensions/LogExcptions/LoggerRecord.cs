using NLog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Extensions.LogExcptions
{
    public class LoggerRecord:ILoggerRecord
    {
        private ILogger _logger;
        private ILogger log = LogManager.GetCurrentClassLogger(typeof(LoggerRecord));
        static readonly ConcurrentDictionary<Type, ILogger> dictLog = new();

        private ILogger GetLogger(Type type)
        {
            if (dictLog.ContainsKey(type))
                return dictLog[type];
            _logger = LogManager.GetCurrentClassLogger(type);
            dictLog.TryAdd(type, _logger);
            return _logger;
        }

        private bool Notexists(Type type, string ex)
        {
            if (type is null || ex is null)
            {
                log.Debug("arguments is null");
                return false;
            };
            _logger = GetLogger(type);
            if (_logger is null)
            {
                log.Debug("_logger 变量为空");
                return false;
            }
            return true;
        }

        public void Debug(Type type, string ex)
        {
            if (!Notexists(type, ex))
                return;
            _logger.Debug(ex);
        }

        public void Info(Type type, string ex)
        {
            if (!Notexists(type, ex))
                return;
            _logger.Info(ex);
        }

        public void Warn(Type type, string ex)
        {
            if (!Notexists(type, ex))
                return;
            _logger.Warn(ex);
        }

        public void Error(Type type, string ex)
        {
            if (!Notexists(type, ex))
                return;
            _logger.Error(ex);
        }
    }
}
