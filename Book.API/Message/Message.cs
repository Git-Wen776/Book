using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Book.API.Message
{
    public class Message<T> where T:class
    {
        public Code code { get; set; }
        public string msg { get; set; }
        public T data { get; set; }

    }

    public enum Code
    {
        Success=200,
        Error=500,
        NotFount=404
    }

    public class Success<T>:Message<T> where T:class
    {
        public Success(T t)
        {
            this.code = Code.Success;
            this.data = t;
            this.msg = "OK";
        }
    }
}
