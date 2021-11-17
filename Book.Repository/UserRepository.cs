using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Book.IRepository;
using Book.Models;
using Book.Extensions.SugarDb;

namespace Book.Repository
{
    public class UserRepository:BaseRepository<User>,IUserRepository
    {
        private readonly IUnitWork work;
        public UserRepository(IUnitWork _work):base(_work)
        {
            work = _work;
        }
    }
}
