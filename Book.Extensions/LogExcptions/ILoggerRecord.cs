using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Extensions.LogExcptions
{
    public interface ILoggerRecord
    {
        void Debug(Type type, string ex);
        void Info(Type type, string ex);
        void Warn(Type type, string ex);
        void Error(Type type, string ex);
    }
}
