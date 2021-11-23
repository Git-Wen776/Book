
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Book.IService;
using Book.Models;

namespace Book.API.Message
{
    public class BaseController:ControllerBase
    {
        protected readonly IUserService _userService;
        private readonly IHttpContextAccessor _contextAccessor;
        public BaseController(IUserService userService,IHttpContextAccessor contextAccessor)
        {
            _userService = userService;
            _contextAccessor = contextAccessor;
        }

        [NonAction]
        public OKResult Success([ActionResultObjectValue] object value)
        {
            return new OKResult(value);
        }
        [NonAction]
        public FailResult Fail([ActionResultObjectValue] object value)
        {
            return new FailResult(value);
        }

        [NonAction]
        public Task<User> GetLoginUser()
        {
            var value = _contextAccessor.HttpContext.User.Claims.FirstOrDefault(p => p.Type.ToString() == "jti").Value;
            return _userService.FindAsync(Convert.ToInt32(value));
        }
    }
    public interface IMessage
    {
        public string Msg { get; set; }
        public Object Data { get; set; }
        public int Code { get; set; }
    }

    public class OKResult : OkObjectResult, IActionResult,IMessage
    {
        public OKResult(Object value) : base(value) {
            Msg = "成功";
            Data = value;
            Code = StatusCodes.Status200OK;
        }

        public async override Task ExecuteResultAsync(ActionContext context)
        {
           await context.HttpContext.Response.WriteAsJsonAsync(new { 
            code=Code,
            data=Data,
            msg=Msg
            });
        }
        public string Msg { get; set; }
        public object Data { get; set; }
        public int Code { get; set; }
    }

    public class FailResult : ObjectResult, IActionResult, IMessage
    {
        public string Msg { get; set; }
        public object Data { get; set; }
        public int Code { get; set; }

        public FailResult(Object value) : base(value) {
            Msg = "失败";
            Data = value;
            Code = StatusCodes.Status400BadRequest;
        }
        public async override Task ExecuteResultAsync(ActionContext context)
        {
            await context.HttpContext.Response.WriteAsJsonAsync(new
            {
                code = Code,
                data = Data,
                msg = Msg
            });
        }
    }
}
