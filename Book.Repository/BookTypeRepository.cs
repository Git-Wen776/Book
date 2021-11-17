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
   public class BookTypeRepository:BaseRepository<BookTypes>,IBookTypeRepsitory
    {
        private readonly IUnitWork work;
        public BookTypeRepository(IUnitWork _work) : base(_work) {
            work = _work;
        }
    }
}
