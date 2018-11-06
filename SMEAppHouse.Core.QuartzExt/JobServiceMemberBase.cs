using System;
using System.Threading;

namespace SMEAppHouse.Core.QuartzExt
{
    public class JobServiceMemberBase<T> : IJobServiceMember
        where T : JobServiceMemberBase<T>
    {
        private Thread _instanceThread;
        public Thread InstanceThread
        {
            get => _instanceThread;
            set => _instanceThread = value;
        }

        public Action InstanceAction { get; set; }
        public bool AsBackground { get; set; }
        public bool AutoRun { get; set; }
        public bool Success { get; set; }
        Exception IJobServiceMember.Exception { get; set; }

        public delegate void SelfDestroyerDelegate(object sender, EventArgs ea);
        public event SelfDestroyerDelegate OnSelfDestructNow;

        public virtual void ExecuteInstance()
        {
            Executing = false;
        }

        public void DestroySelf()
        {
            var selfDestruct = OnSelfDestructNow;
            selfDestruct?.Invoke(this, new EventArgs());
        }

        public bool Executing { get; set; }

        protected JobServiceMemberBase()
        {
            _instanceThread = new Thread(ExecuteInstance)
            {
                IsBackground = AsBackground
            };

            if (AutoRun)
                this.Start();
        }

        public void Start()
        {
            if (_instanceThread == null)
                throw new Exception("Instance thread not initialized. CreateInstance() first.");

            _instanceThread.Start();
            Executing = true;
        }

        //public static T CreateInstance(bool asBackground = true, bool autorun = false)
        //{
        //    var instance = new T()
        //    {
        //        AsBackground = asBackground,
        //        AutoRun = autorun
        //    };

        //    return instance;
        //}
    }
}