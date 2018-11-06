using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using SMEAppHouse.Core.CodeKits;

// ReSharper disable once CheckNamespace
namespace SMEAppHouse.Core.QuartzExt
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JobServiceStarter<T> where T : JobServiceBase<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="recurrenceInterval"></param>
        /// <param name="intervalType"></param>
        /// <param name="jobGroupName"></param>
        /// <param name="triggerGroupName"></param>
        public static async void Start(int recurrenceInterval = 60
            , Rules.TimeIntervalTypesEnum intervalType = Rules.TimeIntervalTypesEnum.Seconds
            , string jobGroupName = ""
            , string triggerGroupName = "")
        {
            try
            {
                // construct a scheduler factory
                // and get a scheduler
                var props = new NameValueCollection
                {
                    { "quartz.serializer.type", "binary" }
                };
                var schedFactory = new StdSchedulerFactory(props);
                var sched = await schedFactory.GetScheduler();
                await sched.Start();

                // define the job and tie it to our IJobService class
                var typeParameterType = typeof(T);
                var jobFor = $"job_for_{typeParameterType.Name}";

                var jbBldr = JobBuilder.Create(typeof(JobServiceInstance<T>));
                var jobDet = jbBldr.WithIdentity(jobFor, string.IsNullOrEmpty(jobGroupName) ? jobFor : jobGroupName)
                                .Build();

                var trgGrpFor = $"trigger_for_{typeParameterType.Name}";
                SimpleScheduleBuilder FuncDefInterval(SimpleScheduleBuilder x, Rules.TimeIntervalTypesEnum iType, int intervalAmt)
                {
                    switch (iType)
                    {
                        case Rules.TimeIntervalTypesEnum.MilliSeconds:
                            return ((SimpleScheduleBuilder)x).WithInterval(TimeSpan.FromMilliseconds(intervalAmt));
                        case Rules.TimeIntervalTypesEnum.Minutes:
                            return x.WithInterval(TimeSpan.FromMinutes(intervalAmt));
                        case Rules.TimeIntervalTypesEnum.Hours:
                            return x.WithInterval(TimeSpan.FromHours(intervalAmt));
                        case Rules.TimeIntervalTypesEnum.Days:
                            return x.WithInterval(TimeSpan.FromDays(intervalAmt));
                        case Rules.TimeIntervalTypesEnum.Seconds:
                            return x.WithInterval(TimeSpan.FromSeconds(intervalAmt));
                    }
                    return x.WithInterval(TimeSpan.FromMilliseconds(intervalAmt));
                }
                // Trigger the job to run now, and then every configured timespan or schedule
                var trigger = TriggerBuilder.Create()
                                .WithIdentity($"trigger_for_{typeParameterType.Name}", string.IsNullOrEmpty(jobGroupName) ? trgGrpFor : triggerGroupName)
                                .StartNow()
                                .WithSimpleSchedule(x => FuncDefInterval(x, intervalType, recurrenceInterval).RepeatForever())
                                .Build();

                await sched.ScheduleJob(jobDet, trigger);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <typeparam name="TJobSvc"></typeparam>
        public class JobServiceInstance<TJobSvc> : IJob
            where TJobSvc : JobServiceBase<TJobSvc>, IJobService
        {
            public JobServiceBase<TJobSvc> Instance => JobServiceBase<TJobSvc>.Instance;

            /// <inheritdoc />
            /// <summary>
            /// </summary>
            /// <param name="context"></param>
            /// <returns></returns>
            public Task Execute(IJobExecutionContext context)
            {
                return Task.Run(() =>
                {
                    JobServiceBase<TJobSvc>.Instance.Execute();
                });
            }
        }

    }


}