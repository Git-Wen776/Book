using Book.IRepository;
using Book.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Book.Extensions.SugarDb;

namespace Book.Repository
{
    public class TaskQzRepository:BaseRepository<TaskQz>,ITaskQzRepository
    {
        public readonly IUnitWork work;
        public TaskQzRepository(IUnitWork _work) : base(_work)
        {
            work = _work;
        }
    }
}
