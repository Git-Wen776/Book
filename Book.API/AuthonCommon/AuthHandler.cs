using Book.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Book.API.AuthonCommon
{
    public class AuthHandler : AuthorizationHandler<PermissonRequirement>
    {
        private readonly IHttpContextAccessor accessor;
        private readonly IAuthorizationService authorizationService;
        private readonly IAuthService auth;
        public AuthHandler(IHttpContextAccessor _accessor,IAuthorizationService _authorizationService,IAuthService _auth)
        {
            accessor = _accessor;   
            authorizationService = _authorizationService;   
            auth = _auth;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissonRequirement requirement)
        {
            throw new System.NotImplementedException();
        }
    }
}
