using Book.IRepository;
using Book.IService;
using Book.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Book.Extensions;
using Book.Extensions.ExpressionExtensions;
using System.Linq.Expressions;

namespace Book.Service
{
    public class AuthService:BaseService<Role>,IAuthService
    {
        private readonly IRoleRepositroy roleRepository;
        private readonly IModularRepository modularRepository;
        private readonly IApiUrlRepositroy apiUrlRepositroy;
        private readonly CreatExprssions creat;

        public AuthService(IRoleRepositroy _role,IModularRepository _modular,IApiUrlRepositroy _apiurl,CreatExprssions _create)
        {
            roleRepository = _role;
            modularRepository = _modular;   
            apiUrlRepositroy = _apiurl; 
            creat = _create;
        }

        public async Task<List<PermissionItem>> GetPermissions()
        {
            Expression<Func<Modular, bool>> modularEx = m => m.IsDeleted == 1;
            Expression<Func<ApiUrl, bool>> apiEx = u => u.IsDeleted == 1;
            var modular =await modularRepository.QueryAsync(modularEx);
            var url = await apiUrlRepositroy.QueryAsync(apiEx);
            var role = await roleRepository.QueryAsync();

            var list = (from m in modular
                       from u in url
                       from r in role
                       where m.ModularId == u.ModularId && m.RId==r.Rid
                       select new PermissionItem
                       {
                           Role=r.RoleName,
                           Url=u.Url
                       }).ToList();
                     

            return list;
        }
    }
}
