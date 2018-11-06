using System;
using System.Collections.Generic;
using System.Threading;

#pragma warning disable 1591

namespace SMED.Core.WebAPI.Patterns.ServiceWrapper
{
    /// <summary>
    /// http://en.wikipedia.org/wiki/Curiously_recurring_template_pattern
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class JobServiceBase<T> : IJobService
        where T : JobServiceBase<T>//, new()
    {
        private static readonly object InstanceMutex = new object();
        private static readonly object IterationMutex = new object();

        private static volatile T _instance;
        private static volatile bool _isSingletonCreation;

        private volatile bool _executing;

        public static T Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                lock (InstanceMutex)
                {
                    if (_instance != null)
                        return _instance;

                    _isSingletonCreation = true;
                    //_instance = new T(); // will require new() in constraint
                    //_instance = (T)Activator.CreateInstance(typeof(T)); 
                    _instance = Activator.CreateInstance<T>();
                    _isSingletonCreation = false;

                    _instance.SubscriberInitialize();
                    return _instance;
                }

            }
        }

        protected void ValidateSingletonCreation()
        {
            if (!_isSingletonCreation)
                throw new ApplicationException("Singleton must not be created.");
        }

        public abstract void SubscriberInitialize();

        public abstract void SubscriberExecute();

        /// <summary>
        /// 
        /// </summary>
        public void Execute()
        {
            lock (IterationMutex)
            {
                if (_executing)
                    return;

                _executing = true;
                var objThread = new List<Thread>
                {
                    new Thread(SubscriberExecute)
                };

                objThread.ForEach(t =>
                {
                    t.Priority = ThreadPriority.Normal;
                    t.Start();
                });

                objThread.ForEach(t => { t.Join(); });
                _executing = false;
            }
        }

    }

}