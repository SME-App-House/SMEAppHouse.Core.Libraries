using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using SMEAppHouse.Core.FreeIPProxy.Handlers;
using SMEAppHouse.Core.ProcessService.Engines;
using SMEAppHouse.Core.ScraperBox;
using SMEAppHouse.Core.ScraperBox.Models;
using ScraperBoxHelper = SMEAppHouse.Core.ScraperBox.Selenium.Helper;
#pragma warning disable 618

namespace SMEAppHouse.Core.FreeIPProxy.Providers.Base
{
    public class IPProxyCartridgeBase : ProcessAgentViaThread, IIPProxyCartridge
    {
        #region private and protected properties

        private IWebDriver _driver = null;

        protected string TargetPgUrl { get; private set; }
        protected bool PageIsValid { get; set; }

        #endregion

        #region public properties

        // Implemented from IIPProxyCartridge
        public string TargetPageUrlPattern { get; private set; }
        public PageInstruction PageInstruction { get; private set; }

        public IList<IPProxy> IPProxies { get; private set; }
        public IPProxyAgentStatusEnum AgentStatus { get; set; }


        public int PageNo { get; private set; }
        public int TotalPagesScraped { get; private set; }

        #endregion

        #region constructors

        public IPProxyCartridgeBase(string targetPageUrlPattern)
            : this(targetPageUrlPattern, null)
        {
        }

        public IPProxyCartridgeBase(string targetPageUrlPattern, int startPageNo)
            : this(targetPageUrlPattern, null, startPageNo)
        {
        }

        public IPProxyCartridgeBase(string targetPageUrlPattern, PageInstruction pgInstruction)
            : this(targetPageUrlPattern, pgInstruction, 0)
        {
        }

        public IPProxyCartridgeBase(string targetPageUrlPattern, PageInstruction pgInstruction, int startPageNo)
        : base(100, true, true, false)
        {
            IPProxies = new List<IPProxy>();
            TargetPageUrlPattern = targetPageUrlPattern;
            PageInstruction = pgInstruction;
            AgentStatus = IPProxyAgentStatusEnum.Idle;
            PageNo = startPageNo == 0 ? 1 : startPageNo;

            var service = PhantomJSDriverService.CreateDefaultService(".\\");
            service.HideCommandPromptWindow = true;

            _driver = new PhantomJSDriver(service);
        }
        ~IPProxyCartridgeBase()
        {
            _driver.Quit();
            _driver.Dispose();
        }

        #endregion

        #region overrides

        /// <summary>
        /// 
        /// </summary>
        protected override void ServiceActionCallback()
        {
            if (AgentStatus == IPProxyAgentStatusEnum.Parsed)
            {
                // scrape next page after 2 minutes.
                base.Suspend(new TimeSpan(0, 0, 25));
                AgentStatus = IPProxyAgentStatusEnum.Idle;
            }

            var usbleProx = SourceTheIPProxy();
            GrabFirstOrNextPage(usbleProx);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual IPProxy SourceTheIPProxy()
        {
            return null;
        }

        /// <summary>
        /// Allow the inheriting client class to validate by checking the content
        /// </summary>
        /// <param name="content"></param>
        protected virtual void ValidatePage(string content)
        {
        }

        /// <summary>
        /// Allow the inheriting client class to parse the content;
        /// From there, need to invoke this base method.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        protected virtual void ParseProxyPage(string content)
        {
            AgentStatus = IPProxyAgentStatusEnum.Parsed;
            // fire the event
            InvokeEventFreeIPProxiesParsed(new EventHandlers.FreeIPProxiesParsedEventArgs(TargetPgUrl, IPProxies.ToList()));
        }

        #endregion

        #region IIPProxyProvider implementation

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proxy"></param>
        public void GrabFirstOrNextPage(IPProxy proxy = null)
        {
            if (AgentStatus != IPProxyAgentStatusEnum.Idle) return;

            var pgNo = PageNo.ToString();
            if (PageInstruction != null)
                pgNo = PageInstruction.PageNo(PageNo);
            TargetPgUrl = TargetPageUrlPattern.Replace("{PAGENO}", pgNo);

            AgentStatus = IPProxyAgentStatusEnum.Reading;
            InvokeEventFreeIPProxiesReading(new EventHandlers.FreeIPProxiesReadingEventArgs(TargetPgUrl));

            // Get the html response called from the url
            var contentDoc = ScraperBoxHelper.GrabPage(_driver as PhantomJSDriver, TargetPgUrl, proxy?.AsTuple());

            ValidatePage(contentDoc);
            if (!PageIsValid)
            {
                AgentStatus = IPProxyAgentStatusEnum.Completed;
                InvokeEventFreeIPProviderSourceCompleted(new EventHandlers.FreeIPProviderSourceCompletedEventArgs(PageNo));
                base.Shutdown();
                return;
            }

            // have the inheriting class parse the html result
            ParseProxyPage(contentDoc);
            TotalPagesScraped++;

            // increase the page no for next use.
            PageNo++;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proxy"></param>
        protected void RegisterProxy(IPProxy proxy)
        {
            AgentStatus = IPProxyAgentStatusEnum.Parsing;
            lock (IPProxies)
            {
                IPProxies.Add(proxy);
            }
            InvokeEventFreeIPProxyParsed(new EventHandlers.FreeIPProxyParsedEventArgs(PageNo, TargetPgUrl, proxy));
        }

        /// <summary>
        /// 
        /// </summary>
        public void ClearProxyBucket()
        {
            lock (IPProxies)
            {
                IPProxies.Clear();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proxy"></param>
        public void RemoveProxy(IPProxy proxy)
        {
            IPProxies.ToList().Remove(proxy);
        }

        #endregion

        #region event handlers

        // implemented from IIPProxyCartridge
        public event EventHandlers.FreeIPProxyParsedEventHandler OnFreeIPProxyParsed;
        public event EventHandlers.FreeIPProxiesParsedEventHandler OnFreeIPProxiesParsed;
        public event EventHandlers.FreeIPGeneratorExceptionEventHandler OnFreeIPGeneratorException;
        public event EventHandlers.FreeIPProxiesReadingEventHandler OnFreeIPProxiesReading;
        public event EventHandlers.FreeIPProviderSourceCompletedEventHandler OnFreeIPProviderSourceCompleted;

        public void InvokeEventFreeIPProxyParsed(EventHandlers.FreeIPProxyParsedEventArgs a)
        {
            OnFreeIPProxyParsed?.Invoke(this, a);
        }

        public void InvokeEventFreeIPProxiesParsed(EventHandlers.FreeIPProxiesParsedEventArgs a)
        {
            OnFreeIPProxiesParsed?.Invoke(this, a);
        }

        public void InvokeEventFreeIPGeneratorException(EventHandlers.FreeIPGeneratorExceptionEventArgs a)
        {
            OnFreeIPGeneratorException?.Invoke(this, a);
        }

        public void InvokeEventFreeIPProxiesReading(EventHandlers.FreeIPProxiesReadingEventArgs a)
        {
            OnFreeIPProxiesReading?.Invoke(this, a);
        }

        public void InvokeEventFreeIPProviderSourceCompleted(EventHandlers.FreeIPProviderSourceCompletedEventArgs a)
        {
            OnFreeIPProviderSourceCompleted?.Invoke(this, a);
        }

        #endregion

    }
}
