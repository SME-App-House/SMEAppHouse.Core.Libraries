using System.Threading;
using SMEAppHouse.Core.TopshelfAdapter.Common;

namespace SMEAppHouse.Core.TopshelfAdapter
{
    public interface ITopshelfClientExt : ITopshelfClient
    {
        Thread ServiceThread { get; }
        //bool LazyInitialization { get; set; }

        event ServiceInitializedEventHandler OnServiceInitialized;
    }
}