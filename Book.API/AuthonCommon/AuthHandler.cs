using Book.Extensions;
using Book.IService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Book.API.AuthonCommon
{
    public class AuthHandler : AuthorizationHandler<PermissonRequirement>
    {
        private readonly IHttpContextAccessor accessor;
        private readonly IAuthorizationService authorizationService;
        private readonly IAuthService auth;
        private readonly IAuthenticationSchemeProvider _schemes;
        public AuthHandler(IHttpContextAccessor _accessor, IAuthorizationService _authorizationService, IAuthService _auth, IAuthenticationSchemeProvider schemes = null)
        {
            accessor = _accessor;
            authorizationService = _authorizationService;
            auth = _auth;
            _schemes = schemes;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissonRequirement requirement)
        {
            var items = auth.GetPermissions();
            var httpcontext = accessor.HttpContext;
            var handlers = httpcontext.RequestServices.GetService<IAuthenticationHandlerProvider>();
            foreach(var item in await _schemes.GetRequestHandlerSchemesAsync())
            {
                var handler=await handlers.GetHandlerAsync(httpcontext,item.Name) as IAuthenticationRequestHandler;
                if(handler != null&& await handler.HandleRequestAsync())
                {
                    context.Fail();
                    return;
                }
            }
            var url = httpcontext.Request.Path.Value;
            bool ispass = httpcontext.User.Identity.IsAuthenticated;//判断是否登录
            if (ispass)
            {
                var list=requirement.Permissions = await items;
                var roles = httpcontext.User.Claims.Where(p => p.Type == ClaimTypes.Role).Select(p => p.Value).ToList();
                foreach (var item in roles)
                {
                    var urls = list.Where(p => p.Role == item).Select(p => p.Url).ToList();
                    if (urls.Any(p => p == url))
                        context.Succeed(requirement);
                }
                var time = httpcontext.User.Claims.Where(p => p.Type == ClaimTypes.Expiration).FirstOrDefault();
                if(time?.Value==null&& DateTime.Parse(time.Value) < DateTime.Now)
                {
                    context.Fail();
                    return;
                }

            }
            else
            {
                context.Fail();
                return ;
            }
            return;
        }
    }
}
