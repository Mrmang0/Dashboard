using Dashboard.Application.Services;
using Dashboard.Domain.Contracts;
using Dashboard.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Infrastrucure.Services
{
    public static class JobRunner
    {
        public static async void Start(IServiceProvider serviceProvider)
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            var fact = serviceProvider.GetService<JobFactory>();
            scheduler.JobFactory = fact;
            await scheduler.Start();

            IJobDetail jobDetail = JobBuilder.Create<DashbordRefreshJob>().Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("DashboardRefresher", "default")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInMinutes(1)
                    .RepeatForever())
                .Build();

            await scheduler.ScheduleJob(jobDetail, trigger);
        }
    }

    public class DashbordRefreshJob : IJob
    {
        private readonly IServiceProvider serviceProvider;

        public DashbordRefreshJob(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task Execute(IJobExecutionContext context)
        {

            var service = serviceProvider.GetService<IDashboradService>();
            var records = await service.GetRecordsAsync();
            foreach (var item in records)
            {
                if (item.NextUpdate < DateTime.UtcNow)
                {
                    var contract = new MakeReportContract()
                    {
                        Description = "Service not started on sceduled time",
                        Name = item.Name,
                        Type = Domain.DashboardEventType.ScheduleCheckFail
                    };
                    await service.MakeReportAsync(contract);
                }
            }

            System.Diagnostics.Debug.WriteLine("I AM RUNNER");
        }
    }

    public class JobFactory : IJobFactory
    {
        protected readonly IServiceProvider serviceProvider;


        public JobFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var job = serviceProvider.GetService(bundle.JobDetail.JobType) as IJob;
            return job;
        }

        public void ReturnJob(IJob job)
        {
            //Do something if need
        }
    }
}
