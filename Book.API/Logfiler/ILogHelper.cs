
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog;

namespace Book.API.Logfiler
{
    public interface ILogHelper
    {
        ILogger GetLogger(Type source);
        void Excpetion(Type source,Exception ex);
    }
}
