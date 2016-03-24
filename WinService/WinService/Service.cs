using Atlas;
using Common.Logging;
using Quartz;
using Quartz.Spi;

namespace WinService
{
    internal class Service : IAmAHostedProcess
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Service));

        public IScheduler Scheduler { get; set; }

        public IJobFactory JobFactory { get; set; }

        public IJobListener JobListener { get; set; }

        public void Start()
        {
            Log.Info("Service starting");

            var job = JobBuilder.Create<Job>()
                    .WithIdentity("Job", "Service")
                    .Build();

            var trigger = TriggerBuilder.Create()
                                        .WithIdentity("Trigger", "Service")
                                        //.WithCronSchedule(ConfigurationManager.AppSettings["CronExpression"])
                                        //.StartAt(DateTime.UtcNow.AddSeconds(5))
                                        .StartNow()
                                        .ForJob("Job", "Service")
                                        .Build();

            Scheduler.JobFactory = JobFactory;
            Scheduler.ScheduleJob(job, trigger);
            Scheduler.ListenerManager.AddJobListener(JobListener);
            Scheduler.Start();
            Log.Info("Processor Service started");
        }

        public void Pause()
        {
            Log.Info("Processor Service pausing");
            Scheduler.PauseAll();
            Log.Info("Processor Service paused");
        }

        public void Resume()
        {
            Log.Info("Processor Service resuming");
            Scheduler.ResumeAll();
            Log.Info("Processor Service resumed");
        }

        public void Stop()
        {
            Log.Info("Processor Service stoping");
            Scheduler.Shutdown();
            Log.Info("Processor Service stoped");
        }
    }
}