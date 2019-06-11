using System.Collections.ObjectModel;

namespace SMEAppHouse.Core.TopshelfAdapter.Aggregation
{
    public interface IServiceController
    {
        ObservableCollection<ITopshelfClientExt> ServiceWorkers { get; }
        void ResumeAll();
        void HaltAll();
        void ShutdownAll();

        //void AddServiceWorker(ITopshelfClientV2 worker);

        event ServiceWorkerInitializedEventHandler OnServiceWorkerInitialized;
    }


}
