using Book.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.IService
{
   public interface IUserService:IBaseService<User>
    {
        Task<List<User>> UserQueryAsync();
        Task<List<User>> GetUsers(User user);
    }

}
