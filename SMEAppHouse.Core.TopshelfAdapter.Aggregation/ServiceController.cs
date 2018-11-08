using System.Collections.ObjectModel;
using System.Linq;
using SMEAppHouse.Core.CodeKits.Data;
using SMEAppHouse.Core.TopshelfAdapter.Common;

namespace SMEAppHouse.Core.TopshelfAdapter.Aggregation
{
    public class ServiceController : IServiceController
    {
        public ObservableCollection<ITopshelfClientExt> ServiceWorkers { get; private set; }
        public event ServiceWorkerInitializedEventHandler OnServiceWorkerInitialized;

        public ServiceController()
        {
            ServiceWorkers = new ObservableCollection<ITopshelfClientExt>();
            ServiceWorkers.CollectionChanged += ServiceWorkers_CollectionChanged;
        }
        ~ServiceController()
        {
            if (ServiceWorkers.Any())
            {
                ServiceWorkers.ForEach(worker =>
                {
                    worker.OnServiceInitialized -= Worker_OnServiceInitialized;
                });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ServiceWorkers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (var newItem in e.NewItems)
            {
                ((ITopshelfClientExt)newItem).OnServiceInitialized += Worker_OnServiceInitialized;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ResumeAll()
        {
            if (!ServiceWorkers.Any()) return;
            ServiceWorkers.ToList().ForEach(svc =>
            {
                svc.Resume();
            });
        }

        public void HaltAll()
        {
            if (!ServiceWorkers.Any()) return;
            ServiceWorkers.ToList().ForEach(svc =>
            {
                svc.Suspend();
            });
        }

        public void ShutdownAll()
        {
            if (!ServiceWorkers.Any()) return;
            ServiceWorkers.ToList().ForEach(svc =>
            {
                svc.Shutdown();
            });
        }

        //public void AddServiceWorker(ITopshelfClientV2 worker)
        //{
        //    lock (ServiceWorkers)
        //    {
        //        worker.OnServiceInitialized += Worker_OnServiceInitialized;
        //        ServiceWorkers.Add(worker);
        //    }
        //}

        private void Worker_OnServiceInitialized(object sender, ServiceInitializedEventArgs e)
        {
            (new ServiceWorkerInitializedEventArgs((ITopshelfClient)sender)).InvokeEvent(this, OnServiceWorkerInitialized);
        }
    }
}
