using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CornJob
{
    public class RememberSchedule
    {
        private readonly IScheduler _scheduler = null;

        public RememberSchedule()
        {
            // 创建作业调度器
            _scheduler = new StdSchedulerFactory().GetScheduler().Result;
            var jobDataMap = new JobDataMap();
            jobDataMap.Add("times", "1");

            // 创建一个作业
            IJobDetail job = JobBuilder.Create<UpdateSearchWordJob>()
                .WithIdentity("job1", "jobGroup1")
                .UsingJobData(jobDataMap)
                .Build();

            // 创建一个触发器
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "triggerGroup1")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(60)
                    .RepeatForever())
                .Build();

            _scheduler.ScheduleJob(job, trigger).Wait();
        }

        public void Start()
        {
            _scheduler.Start().Wait();
        }

        public void Stop()
        {
            _scheduler.Shutdown().Wait();
        }
    }
}
