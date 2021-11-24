using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Book.API.Message;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Collections.Generic;
using Book.API.AuthonCommon;
using Book.IService;
using System.Threading.Tasks;
using Book.Models;
using System.Linq;

namespace Book.API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class LogginController : BaseController
    {
        private readonly IConfiguration config;
        private readonly JwtHelper jwt;
        private readonly IUserService userService;
        private readonly IHttpContextAccessor contextAccessor;
        private readonly IAuthService authService;

        public LogginController(IUserService user,IHttpContextAccessor accessor,IConfiguration _config,JwtHelper _jwt,IAuthService _auth):base(user,accessor)
        { 
          config = _config;
            jwt = _jwt;
            userService = user;
            contextAccessor= accessor;
            authService = _auth;
        }

        [HttpPost(Name = "Logging")]
        public async Task<ActionResult> Logging(string account,string pwd) {
            Book.Models.User user = new Book.Models.User()
            {
                Account = account,
                Password = pwd
            };
            user=await userService.GetuserAsync(user);
            if (user == null)
            {
                return Fail("输入有误");
            }
            var roles =await authService.Roles(user.Id);
            TokenModel tokenModel = new TokenModel() { 
             Uid=1,
             Role=string.Join(',',roles.Select(p=>p.RoleName).ToArray())
            };
            var jwtstr = jwt.TokenStr(tokenModel);
            return Success(jwtstr);
        }
    }
}
