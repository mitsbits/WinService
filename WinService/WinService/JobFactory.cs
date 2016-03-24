using Autofac;
using Quartz;
using Quartz.Spi;
using System;

namespace WinService
{
    internal class JobFactory : IJobFactory
    {
        private readonly IContainer _container;

        private ILifetimeScope _scope;

        public JobFactory(IContainer container)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            _container = container;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            if (bundle == null)
                throw new ArgumentNullException("bundle");
            _scope = _container.BeginLifetimeScope();
            return (IJob)_scope.Resolve(bundle.JobDetail.JobType);
        }

        public void ReturnJob(IJob job)
        {
            _scope.Dispose();
        }
    }
}