using Book.Extensions;
using Book.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace Book.IService
{
    public interface IAuthService:IBaseService<Role>
    {
       public Task<List<PermissionItem>> GetPermissions();

        public Task<List<Role>> Roles(int uid);
    }
}
