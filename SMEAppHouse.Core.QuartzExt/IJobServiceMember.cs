using System;
using System.Threading;

namespace SMEAppHouse.Core.QuartzExt
{
    public interface IJobServiceMember
    {
        bool AsBackground { get; set; }
        bool AutoRun { get; set; }
        bool Success { get; set; }
        Exception Exception { get; set; }
        void Start();
        bool Executing { get; set; }
        Thread InstanceThread { get; set; }
        void ExecuteInstance();
        void DestroySelf();
    }
}