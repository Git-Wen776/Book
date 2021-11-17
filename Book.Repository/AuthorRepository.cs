using Book.Extensions.SugarDb;
using Book.IRepository;
using Book.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Repository
{
    public class AuthorRepository:BaseRepository<Author>,IAuthorRepository
    {
        private readonly IUnitWork work;
        public AuthorRepository(IUnitWork _work):base(_work)
        {
            work = _work;
        }
    }
}
