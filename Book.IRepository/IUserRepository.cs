using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Book.Models;

namespace Book.IRepository
{
   public interface IUserRepository:IBaseRepository<User>
    {
    }
}
