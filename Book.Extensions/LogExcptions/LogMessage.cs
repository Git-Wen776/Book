using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Extensions.LogExcptions
{
   public class LogMessage
    {
        public string IpAddress { get; set; }
        public string AbssemlyName { get; set; }
        public DateTime LogTime { get; set; }
        public string Message { get; set; }
        public string ExInfo { get; set; }
        public string StrackTrace { get; set; }
    }
}
