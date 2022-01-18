using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Tasks
{
    public interface IJobService
    {
        Task StartJob<TJob>(string groupname, string keyname, Func<JobDataMap, ITrigger> createTrigger) where TJob : IJob;
        Task PauseJob(string key, string group);
        Task<bool> JobExists(string key, string group);
        Task ResumeJob(string key, string group);
    }
}
