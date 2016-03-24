using Common.Logging;
using System;
using System.Threading.Tasks;

namespace WinService
{
    internal class Processor : IProcessor, IDisposable
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Processor));

        private readonly Guid _id;

        public Processor()
        {
            _id = Guid.NewGuid();
        }

        public Guid Id
        {
            get
            {
                return _id;
            }
        }

        public async Task Process()
        {
            //database transaction here

            Log.Info("Processed by with id: " + Id.ToString());
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //release resources
            }
        }
    }
}