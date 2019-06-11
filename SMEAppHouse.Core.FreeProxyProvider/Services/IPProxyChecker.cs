using SMEAppHouse.Core.CodeKits.Data;
using SMEAppHouse.Core.ProcessService.Engines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SMEAppHouse.Core.ScraperBox.Models;
using static SMEAppHouse.Core.FreeIPProxy.Handlers.EventHandlers;

namespace SMEAppHouse.Core.FreeIPProxy.Services
{
    public class IPProxyChecker : ProcessAgentViaThread
    {
        private Guid _checkerTokenId = Guid.NewGuid();
        private readonly int _workersCount;

        public IList<IPProxy> ProxyBucket { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IPProxyChecker(int workersCount = 0)
            : base(100, true, true)
        {
            _workersCount = workersCount == 0 ? 1 : workersCount;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prxCnt"></param>
        /// <returns></returns>
        private IList<IPProxy> GrabOpenProxies(int prxCnt)
        {
            lock (ProxyBucket)
            {
                if (ProxyBucket == null || ProxyBucket.Count == 0)
                    return null;

                if (ProxyBucket.All(p => p.CheckerTokenId == _checkerTokenId))
                    return new List<IPProxy>();

                var prxArr = ProxyBucket
                    .Where(p => p.CheckerTokenId != _checkerTokenId)
                    .Take(prxCnt);
                return prxArr.ToList();
            }
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="proxy"></param>
        //private void TestAndUpdateProxy(IPProxy proxy)
        //{
        //    var isValid = false;
        //    proxy.CheckStatus = IPProxy.CheckStatusEnum.Checking;
        //    if (Helper.ProxyIsGood(proxy.IPAddress, proxy.PortNo))
        //    {
        //        proxy.CheckerTokenId = _checkerTokenId;
        //        proxy.LastChecked = DateTime.Now;
        //        proxy.CheckStatus = IPProxy.CheckStatusEnum.Checked;

        //        isValid = true;
        //    }
        //    else proxy.CheckStatus = IPProxy.CheckStatusEnum.CheckedInvalid;
        //    OnIPProxyChecked?.Invoke(this, new IPProxyCheckedEventArgs(isValid, proxy));
        //}

        /// <summary>
        /// 
        /// </summary>
        protected override void ServiceActionCallback()
        {
            if (ProxyBucket == null || ProxyBucket.Count == 0) return;
            var opnPrxs = GrabOpenProxies(_workersCount);

            if (!opnPrxs.Any())
                return;

            var prxTaskList = new List<Task>();
            foreach (var prx in opnPrxs)
            {
                var tsk = new Task(() =>
                {
                    var isValid = Helper.TestIPProxy2(prx);
                    OnIPProxyChecked?.Invoke(this, new IPProxyCheckedEventArgs(isValid, prx));
                });
                tsk.Start();
                prxTaskList.Add(tsk);
            }
            Task.WaitAll(prxTaskList.ToArray());

            lock (ProxyBucket)
            {
                var removedCnt = ProxyBucket.Remove(proxy => proxy.CheckStatus == IPProxy.CheckStatusEnum.CheckedInvalid);
                OnIPProxiesChecked?.Invoke(this, new IPProxiesCheckedEventArgs(removedCnt));
            }

            if (ProxyBucket.Any(p => p.CheckerTokenId != _checkerTokenId))
                return;

            // resume to repeat in 1 minutes
            base.Suspend(new TimeSpan(0, 1, 0));

            // renew the process token;
            _checkerTokenId = Guid.NewGuid();
        }

        /// <summary>
        /// 
        /// </summary>
        public event IPProxyCheckedEventHandler OnIPProxyChecked;
        public event IPProxiesCheckedEventHandler OnIPProxiesChecked;

    }
}
