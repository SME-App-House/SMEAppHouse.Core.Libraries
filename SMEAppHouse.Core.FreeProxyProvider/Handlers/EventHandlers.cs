using System;
using System.Collections.Generic;
using SMEAppHouse.Core.ScraperBox.Models;

namespace SMEAppHouse.Core.FreeIPProxy.Handlers
{
    public class EventHandlers
    {

        public class FreeIPProxiesReadingEventArgs : EventArgs
        {
            public string TargetPageUrl { get; }

            public FreeIPProxiesReadingEventArgs(string targetPageUrl)
            {
                TargetPageUrl = targetPageUrl;
            }
        }
        public delegate void FreeIPProxiesReadingEventHandler(object sender, FreeIPProxiesReadingEventArgs e);

        public class FreeIPProxyParsedEventArgs : EventArgs
        {
            public string TargetPageUrl{ get; }
            public IPProxy IPProxy { get; }
            public int PageNo { get; }

            public FreeIPProxyParsedEventArgs(int pageNo, string targetPageUrl, IPProxy proxy)
            {
                TargetPageUrl = targetPageUrl;
                IPProxy = proxy;
                PageNo = pageNo;
            }
        }
        public delegate void FreeIPProxyParsedEventHandler(object sender, FreeIPProxyParsedEventArgs e);

        public class FreeIPProxiesParsedEventArgs : EventArgs
        {
            public string TargetPageUrl { get; }
            public List<IPProxy> IPProxies { get; set; }

            public FreeIPProxiesParsedEventArgs(string targetPageUrl, List<IPProxy> proxies)
            {
                TargetPageUrl = targetPageUrl;
                IPProxies = proxies;
            }
        }
        public delegate void FreeIPProxiesParsedEventHandler(object sender, FreeIPProxiesParsedEventArgs e);

        public class FreeIPGeneratorExceptionEventArgs : EventArgs
        {
            public Exception Exception { get; set; }

            public FreeIPGeneratorExceptionEventArgs(Exception exception)
            {
                Exception = exception;
            }
        }
        public delegate void FreeIPGeneratorExceptionEventHandler(object sender, FreeIPGeneratorExceptionEventArgs e);

        public class IPProxyCheckedEventArgs : EventArgs
        {
            public bool IsValid { get; set; }
            public IPProxy IPProxy { get; set; }

            public IPProxyCheckedEventArgs(bool isValid, IPProxy proxy)
            {
                IsValid = isValid;
                IPProxy = proxy;
            }
        }
        public delegate void IPProxyCheckedEventHandler(object sender, IPProxyCheckedEventArgs e);

        public class IPProxiesCheckedEventArgs : EventArgs
        {
            public int RemovedProxies { get; set; }

            public IPProxiesCheckedEventArgs(int removedProxies)
            {
                RemovedProxies= removedProxies;
            }
        }
        public delegate void IPProxiesCheckedEventHandler(object sender, IPProxiesCheckedEventArgs e);

        public delegate void IPProxyReadyEventHandler(object sender, EventArgs e);


        public class FreeIPProviderSourceCompletedEventArgs : EventArgs
        {
            public int TotalPages { get; }

            public FreeIPProviderSourceCompletedEventArgs(int totalPages)
            {
                TotalPages = totalPages;
            }
        }
        public delegate void FreeIPProviderSourceCompletedEventHandler(object sender, FreeIPProviderSourceCompletedEventArgs e);

        public class FreeIPProviderSourcesCompletedEventArgs : EventArgs
        {
            public int TotalPages { get; }

            public FreeIPProviderSourcesCompletedEventArgs(int totalPages)
            {
                TotalPages = totalPages;
            }
        }
        public delegate void FreeIPProviderSourcesCompletedEventHandler(object sender, FreeIPProviderSourcesCompletedEventArgs e);

    }
}
