using Atlas;
using Autofac;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System.Linq;
using System.Reflection;

namespace WinService
{
    internal class ServiceModule : Autofac.Module
    {
        static ServiceModule()
        {
            //get app config values
        }

        protected override void Load(ContainerBuilder builder)
        {
            WireUpQuartz(builder);
            WireUpServices(builder);
            WireUpDbContexts(builder);
            WireUpLogicLayers(builder);
        }

        private void WireUpQuartz(ContainerBuilder builder)
        {
            builder.Register(c => new StdSchedulerFactory().GetScheduler())
                   .As<IScheduler>()
                   .InstancePerLifetimeScope();
            builder.Register(c => new JobFactory(ContainerProvider.Instance.ApplicationContainer))
                   .As<IJobFactory>();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                   .Where(p => typeof(IJob).IsAssignableFrom(p))
                   .PropertiesAutowired();
            builder.Register(c => new JobListener(ContainerProvider.Instance))
                   .As<IJobListener>();
        }

        private void WireUpServices(ContainerBuilder builder)
        {
            builder.RegisterType<Service>()
                   .As<IAmAHostedProcess>()
                   .PropertiesAutowired();
        }

        private static void WireUpDbContexts(ContainerBuilder builder)
        {
            // wire up ef
        }

        private void WireUpLogicLayers(ContainerBuilder builder)
        {
            builder.RegisterType<Processor>()
                   .As<IProcessor>().InstancePerLifetimeScope();  //.SingleInstance();
        }
    }
}