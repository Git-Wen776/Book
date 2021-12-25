using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Book.IService;
using Quartz;

namespace Book.Tasks
{
    public class JobBase
    {
        protected Task JobExcuete(IJobExecutionContext context,Func<Task> func)
        {
          return  Task.CompletedTask;
        }

    }
}
