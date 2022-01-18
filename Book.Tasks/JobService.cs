using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Tasks
{
    public class JobService:IJobService
    {
        private readonly Task<IScheduler> _scheduler;
        private readonly ISchedulerFactory _factory;

        public JobService(ISchedulerFactory factory)
        {
            _factory = factory;
            _scheduler = _factory.GetScheduler();
        }

        /// <summary>
        /// 开启任务
        /// </summary>
        /// <typeparam name="TJob"></typeparam>
        /// <param name="groupname"></param>
        /// <param name="keyname"></param>
        /// <param name="createTrigger"></param>
        /// <returns></returns>
        public async Task StartJob<TJob>(string groupname, string keyname, Func<JobDataMap, ITrigger> createTrigger) where TJob : IJob
        {
            if (await JobExists(keyname, groupname))
                return;
            IJobDetail job = JobBuilder.Create<TJob>()
               .WithIdentity(keyname, groupname)
               .Build();
            JobDataMap jobDataMap = new JobDataMap();
            ITrigger trigger = createTrigger(jobDataMap);
            await _scheduler?.Result?.ScheduleJob(job, trigger);
        }

        /// <summary>
        /// 判断任务是否执行存在
        /// </summary>
        /// <param name="key"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public Task<bool> JobExists(string key, string group)
        {
            var jobkey = new JobKey(key, group);
            return _scheduler?.Result?.CheckExists(jobkey);
        }

        /// <summary>
        /// 暂停任务
        /// </summary>
        /// <param name="key"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public async Task PauseJob(string key, string group)
        {
            if (!await JobExists(key, group))
                return;
            var jobkey = new JobKey(key, group);
            await _scheduler?.Result?.PauseJob(jobkey);
        }

        /// <summary>
        /// 恢复任务
        /// </summary>
        /// <param name="key"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public async Task ResumeJob(string key, string group)
        {
            if (await JobExists(key, group))
                return;
            var jobkey = new JobKey(key, group);
            await _scheduler?.Result?.ResumeJob(jobkey);
        }
    }
}
