using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SMEAppHouse.Core.AppMgt.Messaging;

namespace SMEAppHouse.Core.AppMgt.ServiceTemplate
{
    public abstract class ServiceStarter<T> : IServiceStarter<T> where T : class
    {
        public ILogger<T> Logger { get; set; }
        public IConfiguration Configuration { get; set; }
        public IMapper Mapper { get; set; }
        public IPayloadsEnvelope PayloadsEnvelope { get; set; }
        public int TaskIntervalInSeconds { get; set; } = 60; // default interval will be 60 seconds
        public ServicePulseBehaviorEnum PulseBehavior { get; set; } = ServicePulseBehaviorEnum.Synchronous;

        private Timer _timer;
        private volatile bool _executing;
        private static readonly object IterationMutex = new object();

        #region constructors

        protected ServiceStarter(IConfiguration config, ILogger<T> logger, IMapper mapper)
            : this(config, logger, mapper, null, 60)
        {
        }

        protected ServiceStarter(IConfiguration config, ILogger<T> logger, IMapper mapper, IPayloadsEnvelope pyloadEnv)
            : this(config, logger, mapper, pyloadEnv, 60)
        {
        }

        protected ServiceStarter(IConfiguration config, ILogger<T> logger, IMapper mapper, int taskIntervalInSeconds)
            : this(config, logger, mapper, null, taskIntervalInSeconds)
        {
        }

        protected ServiceStarter(IConfiguration config, ILogger<T> logger, IMapper mapper, IPayloadsEnvelope pyloadEnv, int taskIntervalInSeconds)
        {
            Logger = logger;
            Configuration = config;
            Mapper = mapper;
            PayloadsEnvelope = pyloadEnv;
            TaskIntervalInSeconds = taskIntervalInSeconds;
        }

        #endregion

        public abstract void PerformServiceTask();

        private void DoWork(object state)
        {
            lock (IterationMutex)
            {
                if (_executing)
                    return;

                _executing = true;

                Logger.LogInformation($"Service is working for {typeof(T).Name}.");

                if (PulseBehavior == ServicePulseBehaviorEnum.Asynchronous)
                {
                    var threadCtxt = SynchronizationContext.Current ?? new SynchronizationContext();
                    threadCtxt.Send(s =>
                    {
                        PerformServiceTask();
                    }, null);
                }
                else PerformServiceTask();

                _executing = false;
            }
        }

        public Task Execute(CancellationToken cancellationToken)
        {
            Logger.LogInformation($"{nameof(T)} service is starting.");
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(TaskIntervalInSeconds));
            return Task.CompletedTask;
        }

        public Task Terminate(CancellationToken cancellationToken)
        {
            Logger.LogInformation($"{ nameof(T)} service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

    }
}
