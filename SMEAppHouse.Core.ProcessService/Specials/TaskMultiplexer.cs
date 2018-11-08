using System.Collections.Generic;
using System.Threading.Tasks;
using SMEAppHouse.Core.ProcessService.Engines;

namespace SMEAppHouse.Core.ProcessService.Specials
{
    public class TaskMultiplexer<T> : ProcessAgentViaTask where T : class
    {
        private volatile Queue<TaskSlug> _fifoTargets = new Queue<TaskSlug>();
        private volatile int _numOfWorkersFree;

        public int NumberOfWorkers { get; protected set; }

        public Queue<TaskSlug> FIFOTargets
        {
            get { return _fifoTargets; }
            set { _fifoTargets = value; }
        }

        public TaskMultiplexer(int numOfWorkers = 1)
        {
            NumberOfWorkers = numOfWorkers;
        }

        public virtual bool EnactOnTarget(T target)
        {
            return false;
        }

        protected override void ServiceActionCallback()
        {
            if(_numOfWorkersFree==NumberOfWorkers) 
                return;

            Task.Factory.StartNew(() =>
            {
                _numOfWorkersFree++;
                //if(EnactOnTarget())
                _numOfWorkersFree--;
            });

        }

        public class TaskSlug
        {
            public bool Success { get; set; }
            public T Target { get; set; }
            public TaskSlug(T target)
            {
                Target = target;
            }
        }
    }


}
