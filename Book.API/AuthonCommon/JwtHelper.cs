using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Book.API.AuthonCommon
{
    public class JwtHelper
    {
        private readonly IConfiguration config;
        public JwtHelper(IConfiguration _config){
            config = _config;
        }
        public string TokenStr(TokenModel model) {
            string iss = config.GetSection("Authen:Isusser").Value;
            string screat = config.GetSection("Authen:SingingKey").Value;
            string aud = config.GetSection("Authen:Audience").Value;
            string jwtstr = string.Empty;
            if (model != null) {
                List<Claim> claims = new List<Claim>(){ 
                    new Claim(JwtRegisteredClaimNames.Jti,model.Uid.ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
                    new Claim(JwtRegisteredClaimNames.Nbf, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
                    //这个就是过期时间，目前是过期1000秒，可自定义，注意JWT有自己的缓冲过期时间
                    new Claim(JwtRegisteredClaimNames.Exp, $"{new DateTimeOffset(DateTime.Now.AddMinutes(10)).ToUnixTimeSeconds()}"),
                    new Claim(JwtRegisteredClaimNames.Iss,iss),
                    new Claim(JwtRegisteredClaimNames.Aud,aud)
                };
                if (model.Role.Contains(","))
                {
                    claims.AddRange(model.Role.Split(',').Select(p => new Claim(ClaimTypes.Role, p)));
                }
                else {
                    claims.Add(new Claim(ClaimTypes.Role, model.Role));
                }
                var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(screat));
                var scrkey = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var jwt = new JwtSecurityToken(
                    issuer:iss,
                    claims:claims,
                    signingCredentials:scrkey
                    );
                var handler = new JwtSecurityTokenHandler();
                jwtstr=handler.WriteToken(jwt);
            }
            return jwtstr;
        }

        public TokenModel SerilaizeToken(string token) {

            return new TokenModel
            {
                Uid=1,
                Role="User"
            };
        }
        
    }
    public class TokenModel
    {
        public int Uid { get; set; }
        public string Role { get; set; }    
    }
}
