using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Book.API.Message;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Collections.Generic;
using Book.API.AuthonCommon;

namespace Book.API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class LogginController : BaseController
    {
        private readonly IConfiguration config;
        private readonly JwtHelper jwt;

        public LogginController(IConfiguration _config,JwtHelper _jwt) { 
          config = _config;
            jwt = _jwt;
        }

        [HttpGet(Name = "Logging")]
        public ActionResult Logging() {

            TokenModel tokenModel = new TokenModel() { 
             Uid=1,
             Role="User"
            };
            var jwtstr = jwt.TokenStr(tokenModel);
            return Success(jwtstr);
        }
    }
}
