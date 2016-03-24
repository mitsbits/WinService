using Common.Logging;
using Quartz;

namespace WinService
{
    internal class Job : IJob
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Job));

        private readonly IProcessor _processor;

        public Job(IProcessor processor)
        {
            _processor = processor;
        }

        public void Execute(IJobExecutionContext context)
        {
            Log.Info("Job executing");

            _processor.Process();

            Log.Info("Job executed");
        }
    }
}