using SMEAppHouse.Core.FreeIPProxy.Providers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SMEAppHouse.Core.CodeKits.Data;
using SMEAppHouse.Core.ScraperBox.Models;
using SMEAppHouse.Core.ScraperBox.Selenium;
using static SMEAppHouse.Core.FreeIPProxy.Handlers.EventHandlers;

namespace SMEAppHouse.Core.FreeIPProxy.Services
{
    public class IPProxyManager
    {
        private IPProxyChecker _proxyCheckerSvc;
        private bool _checkerIsRunning;
        private bool _firedReady;

        public IIPProxyCartridge[] ProxyCartridges { get; set; }
        public int IPCheckWorkerCounts { get; set; }
        public List<IPProxy> ProxyBucket { get; set; }

        public event IPProxyCheckedEventHandler OnIPProxyChecked;
        public event IPProxiesCheckedEventHandler OnIPProxiesChecked;
        public event FreeIPProxiesReadingEventHandler OnFreeIPProxiesReading;
        public event FreeIPProxiesParsedEventHandler OnFreeIPProxiesParsed;
        public event FreeIPProxyParsedEventHandler OnFreeIPProxyParsed;
        public event IPProxyReadyEventHandler OnIPProxyReady;
        public event FreeIPProviderSourceCompletedEventHandler OnFreeIPProviderSourceCompleted;
        public event FreeIPProviderSourcesCompletedEventHandler OnFreeIPProviderSourcesCompleted;

        public IPProxyManager(params IIPProxyCartridge[] proxyCartridges)
            : this(false, proxyCartridges)
        { }
        public IPProxyManager(bool autoIpCheck, params IIPProxyCartridge[] proxyCartridges)
            : this(autoIpCheck, 0, proxyCartridges)
        {
        }
        public IPProxyManager(bool autoIpCheck, int ipCheckWorkerCounts, params IIPProxyCartridge[] proxyCartridges)
        {
            ProxyBucket = new List<IPProxy>();

            IPCheckWorkerCounts = ipCheckWorkerCounts;
            ProxyCartridges = proxyCartridges;

            foreach (var c in ProxyCartridges)
            {
                c.OnFreeIPProviderSourceCompleted += (s, e) =>
                {
                    if (ProxyCartridges.All(p => p.AgentStatus == IPProxyAgentStatusEnum.Completed))
                    {
                        OnFreeIPProviderSourcesCompleted?.Invoke(this,
                            new FreeIPProviderSourcesCompletedEventArgs(ProxyCartridges.Sum(p => p.TotalPagesScraped)));
                    }
                    else
                    {
                        OnFreeIPProviderSourceCompleted?.Invoke(s,
                            new FreeIPProviderSourceCompletedEventArgs(e.TotalPages));
                    }
                };
                c.OnFreeIPProxiesReading += (s, e) => OnFreeIPProxiesReading?.Invoke(s, e);
                c.OnFreeIPProxiesParsed += (s, e) =>
                 {
                     foreach (var prx in e.IPProxies)
                     {
                         lock (ProxyBucket)
                         {
                             ProxyBucket.Add(prx);
                         }
                     }
                     ((IIPProxyCartridge)s).ClearProxyBucket();
                     OnFreeIPProxiesParsed?.Invoke(s, e);
                 };
                c.OnFreeIPProxyParsed += (s, e) => OnFreeIPProxyParsed?.Invoke(s, e);
            }

            if (autoIpCheck)
                RunChecker();
        }
        ~IPProxyManager()
        {
            if (_checkerIsRunning)
                StopChecker();
        }

        public bool IsReady => ProxyBucket.Any(p => p.CheckStatus == IPProxy.CheckStatusEnum.Checked);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IPProxy GetOne(bool pop = false)
        {
            lock (ProxyBucket)
            {
                IPProxy proxy;
                if (!pop)
                    proxy = ProxyBucket.FirstOrDefault(p => p.CheckStatus == IPProxy.CheckStatusEnum.Checked);
                else proxy = ProxyBucket
                            .Where(p => p.CheckStatus == IPProxy.CheckStatusEnum.Checked).ToList()
                            .Pop();
                return proxy;
            }
        }

        public void Start()
        {
            foreach (var c in ProxyCartridges)
            {
                Task.Delay(1000)
                    .ContinueWith(t =>
                    {
                        c.Resume();
                    });
            }
        }

        public void Stop()
        {
            foreach (var c in ProxyCartridges)
            {
                Task.Delay(1000)
                    .ContinueWith(t =>
                    {
                        c.Suspend();
                    });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void RunChecker()
        {
            if (_proxyCheckerSvc == null)
                _proxyCheckerSvc = new IPProxyChecker(IPCheckWorkerCounts)
                {
                    ProxyBucket = ProxyBucket
                };

            _proxyCheckerSvc.OnIPProxyChecked += (s, e) =>
            {
                OnIPProxyChecked?.Invoke(this, e);

                // fire ready event
                if (!e.IsValid || ProxyBucket.Count <= 0 || _firedReady)
                    return;

                _firedReady = true;
                OnIPProxyReady?.Invoke(this, new EventArgs());
            };

            _proxyCheckerSvc.OnIPProxiesChecked += (s, e) =>
            {
                OnIPProxiesChecked?.Invoke(this, e);
            };

            Task.Delay(1000)
                .ContinueWith(t =>
                {
                    _proxyCheckerSvc.Resume();
                    _checkerIsRunning = true;
                });
        }

        /// <summary>
        /// 
        /// </summary>
        private void StopChecker()
        {
            if (_proxyCheckerSvc == null) return;
            _proxyCheckerSvc.Shutdown();
            _proxyCheckerSvc.OnIPProxyChecked -= null;
            _proxyCheckerSvc = null;
            _checkerIsRunning = false;
        }

    }
}