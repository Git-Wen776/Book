using Book.Extensions.SugarDb;
using Book.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Book.API.Message;
using Book.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;

namespace Book.API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUnitWork work;
        private readonly IUserService userservice;
        private readonly ILogger<UserController> logger;
        private readonly IConfiguration config;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMapper _mapper;

        public UserController(IUserService _service,IHttpContextAccessor accessor,IUnitWork _work, ILogger<UserController> _logger,IConfiguration _configuration,IMapper mapper):base(_service,accessor)
        {
            work = _work;
            userservice = _service;
            logger = _logger;
            config = _configuration;
            httpContextAccessor = accessor;
            _mapper = mapper;
        }

        [HttpGet(Name = "GetUser")]
        public async Task<ActionResult> GetUser(int id)
        {
            
            return Success(await userservice.FindAsync(id));
        }

        [HttpGet(Name ="GetPorts")]
        [Authorize(Policy = "BookPolicy")]
        public ActionResult GetPorts() {
            List<string> list = new();
            config.Bind("AllowPorts", list);
            return Success(list);
        }

        [HttpGet(Name = "GetUsers")]
        public async Task<ActionResult> GetUsers() {
            
            
            var list = await userservice.UserQueryAsync();
            if (list.Count == 0) {
                return Success("暂无数据");
            }
            return Success(list);   
        }

        [HttpGet(Name = "FindUser")]
        public async Task<ActionResult> FindUser(string name)
        {
            User user = new User() { Name = name };
            return Success(await userservice.GetUsers(user));
        }

        [HttpGet(Name = "Todate")]
        public ActionResult Todate(string date)
        {
            return Success(Convert.ToDateTime(date));
        }
    }
}
