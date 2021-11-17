using Book.IRepository;
using Book.IService;
using Book.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Service
{
    public class AuthService:BaseService<Role>,IAuthService
    {
        private readonly IRoleRepositroy roleRepository;
        private readonly IModularRepository modularRepository;
        private readonly IApiUrlRepositroy apiUrlRepositroy;    

        public AuthService(IRoleRepositroy _role,IModularRepository _modular,IApiUrlRepositroy _apiurl)
        {
            roleRepository = _role;
            modularRepository = _modular;   
            apiUrlRepositroy = _apiurl; 
        }
    }
}
