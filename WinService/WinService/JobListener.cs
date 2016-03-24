using Atlas;
using Quartz;
using System;

namespace WinService
{
    internal class JobListener : IJobListener
    {
        private readonly IContainerProvider _provider;

        private IUnitOfWorkContainer _container;

        public JobListener(IContainerProvider provider)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

            _provider = provider;
            Name = "JobListener";
        }

        public string Name { get; private set; }
        public object ConfigurationManager { get; private set; }

        public void JobToBeExecuted(IJobExecutionContext context)
        {
            _container = _provider.CreateUnitOfWork();
            _container.InjectUnsetProperties(context.JobInstance);
        }

        public void JobExecutionVetoed(IJobExecutionContext context)
        {
        }

        public void JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException)
        {
            _container.Dispose();

            //reschecule job IntervalInSeconds
            var trigger = TriggerBuilder.Create()
                .WithIdentity("Trigger", "Service")
                //.WithCronSchedule(ConfigurationManager.AppSettings["CronExpression"])
                .StartAt(DateTime.UtcNow.AddSeconds(int.Parse(System.Configuration.ConfigurationManager.AppSettings["IntervalInSeconds"])))
                .ForJob("Job", "Service")
                .Build();

            var triggerKey = context.Trigger.Key;
            context.Scheduler.RescheduleJob(triggerKey, trigger);
        }
    }
}