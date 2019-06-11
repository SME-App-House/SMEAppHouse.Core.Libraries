using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using SMEAppHouse.Core.ProcessService.Engines;
using SMEAppHouse.Core.CodeKits.Extensions;
using SMEAppHouse.Core.CodeKits;
using SMEAppHouse.Core.FreeIPProxy.Handlers;
using SMEAppHouse.Core.FreeIPProxy.Models;
using OpenQA.Selenium.PhantomJS;

namespace SMEAppHouse.Core.FreeIPProxy.Providers.Base
{
    public abstract class IPProxyProviderBase : ProcessAgentViaTask, IIPProxyProvider, IDisposable
    {
        #region private variables

        private int _pgCounter;
        private IWebDriver _driver;

        #endregion

        #region constructors

        public IPProxyProviderBase()
            // we will query the provider every five minutes
            : base(new TimeSpan(0, 0, 1, 0))
        {

        }

        public IPProxyProviderBase(bool consumeSelf)
            : this()
        {
            ConsumeSelf = consumeSelf;
        }

        #endregion

        #region event handlers

        public event EventHandlers.FreeIPProxyFetchedEventHandler OnFreeIPProxyFetched;
        public event EventHandlers.FreeIPProxiesFetchedEventHandler OnFreeIPProxiesFetched;
        public event EventHandlers.FreeIPGeneratorExceptionEventHandler OnFreeIPGeneratorException;

        public void RemoveProxy(FreeProxy proxy)
        {

        }

        public bool RemoveFreeProxy(FreeProxy proxy)
        {
            return FreeProxies.ToList().Remove(proxy);
        }

        public void InvokeEventFreeIPProxyFetched(EventHandlers.FreeIPProxyFetchedEventArgs a)
        {
            var handler = OnFreeIPProxyFetched;
            if (handler != null)
                handler(this, a);
        }

        public void InvokeEventFreeIPProxiesFetched(EventHandlers.FreeIPProxiesFetchedEventArgs a)
        {
            var handler = OnFreeIPProxiesFetched;
            if (handler != null)
                handler(this, a);
        }

        public void InvokeEventFreeIPGeneratorException(EventHandlers.FreeIPGeneratorExceptionEventArgs a)
        {
            var handler = OnFreeIPGeneratorException;
            if (handler != null)
                handler(this, a);
        }

        #endregion

        #region IIPProxyProvider implementation

        public string HostPageUrlPattern { get; set; }

        public int HostPageMax { get; set; }

        public Queue<FreeProxy> FreeProxies { get; set; }

        public bool ConsumeSelf { get; set; }

        public FreeProxy GetFreeProxy(bool dequeue = true)
        {
            if (FreeProxies == null) return null;
            if (!dequeue)
            {
                var idx = CodeKit.RandomNumber(0, FreeProxies.Count);
                return FreeProxies.ToList()[idx];
            }
            else
            {
                var proxy = FreeProxies.Dequeue();
                return proxy;
            }
        }

        public virtual Queue<FreeProxy> LoadUpIPProxies(string pageContentDocument, Action<FreeProxy> freeProxyFetched)
        {
            throw new InvalidOperationException("Base method must not execute.");
        }

        #endregion

        #region ProcessAgentViaTask implementation

        protected override void ServiceActionCallback()
        {
            if (FreeProxies == null)
                FreeProxies = new Queue<FreeProxy>();

            FreeProxy freeProxy = null;

            if (ConsumeSelf && FreeProxies.Any())
            {
                freeProxy = FreeProxies.ToArray()[0];
            }

            _pgCounter += 1;
            var hostPgUrlPattern = MakeHostPageUrlPattern(HostPageUrlPattern, _pgCounter);

            if (string.IsNullOrEmpty(hostPgUrlPattern))
                throw new InvalidOperationException("Failed to parse HostPgUrlPattern to produce valid url.");

            try
            {
                var contentDoc = LoadIPProxyDocumentContent(hostPgUrlPattern, freeProxy);

                if (!string.IsNullOrEmpty(contentDoc))
                {
                    var freeProxies = LoadUpIPProxies(contentDoc, (prxy) =>
                    {
                        FreeProxies.Enqueue(prxy);
                        InvokeEventFreeIPProxyFetched(new EventHandlers.FreeIPProxyFetchedEventArgs(prxy));
                    });

                    if (freeProxies == null || !freeProxies.Any()) return;
                    //FreeProxies = FreeProxies.Concat(freeProxies).ToList();
                    InvokeEventFreeIPProxiesFetched(new EventHandlers.FreeIPProxiesFetchedEventArgs(freeProxies.ToList()));
                }

            }
            catch (Exception ex)
            {
                if (freeProxy != null)
                {
                    lock (FreeProxies)
                        RemoveFreeProxy(freeProxy);
                }

                _pgCounter--;
                return;
            }

            if (_pgCounter == HostPageMax)
                _pgCounter = 0;

        }

        #endregion

        #region public statics

        /// <summary>
        /// 
        /// </summary>
        /// <param name="partial"></param>
        /// <returns></returns>
        protected static ProxyCountriesEnum FindProxyCountryFromPartial(string countryNamePartial)
        {
            return Helper.FindProxyCountryFromPartial(countryNamePartial);
        }

        /// <summary>
        /// Get paging format for uri 
        /// Example: '{0-2-1}' means : '0' character at '2' digits pad to left(0)/right(1)
        /// </summary>
        /// <param name="hostPageUrlPattern"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        protected static string MakeHostPageUrlPattern(string hostPageUrlPattern, int pageNo)
        {
            try
            {
                if (hostPageUrlPattern.Contains("{PAGENO}"))
                {
                    return hostPageUrlPattern.Replace("{PAGENO}", pageNo.ToString());
                }
                else
                {
                    var start = hostPageUrlPattern.IndexOf("{", StringComparison.Ordinal);
                    var end = hostPageUrlPattern.IndexOf("}", StringComparison.Ordinal);
                    var userFormat = hostPageUrlPattern.Substring(start, end - start).Replace("{", "").Replace("}", "");
                    var members = userFormat.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);

                    if (members.Count() < 3)
                        throw new InvalidOperationException(
                            "MakeHostPageUrlPattern failed: hostPageUrlPattern has invalid page formatting directive");

                    var pgNo = string.Empty;

                    if (members[2].Equals("0"))
                        pgNo = pageNo.ToString().PadRight(int.Parse(members[1]), char.Parse(members[0]));

                    if (members[2].Equals("1"))
                        pgNo = pageNo.ToString().PadLeft(int.Parse(members[1]), char.Parse(members[0]));

                    if (string.IsNullOrEmpty(pgNo))
                        throw new InvalidOperationException(
                            "MakeHostPageUrlPattern failed: hostPageUrlPattern has invalid page formatting directive");

                    userFormat = "{[uF]}".Replace("[uF]", userFormat);
                    return hostPageUrlPattern.Replace(userFormat, pgNo);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostPgUrlPattern"></param>
        /// <param name="freeProxy"></param>
        /// <returns></returns>
        protected string LoadIPProxyDocumentContent(string hostPgUrlPattern, FreeProxy freeProxy = null)
        {
            /*
             REFERENCES:
                other approach: 
                    //http://stackoverflow.com/questions/18921099/add-proxy-to-phantomjsdriver-selenium-c
                    PhantomJSOptions phoptions = new PhantomJSOptions();
                    phoptions.AddAdditionalCapability(CapabilityType.Proxy, "http://localhost:9999");
                    driver = new PhantomJSDriver(phoptions);

                - (24/04/2019) https://stackoverflow.com/a/52446115/3796898
             */

            try
            {

                //var options = new ChromeOptions();
                //var userAgent = "user_agent_string";
                //options.AddArgument("--user-agent=" + userAgent);
                //IWebDriver driver = new ChromeDriver(options);

                var service = PhantomJSDriverService.CreateDefaultService();
                service.HideCommandPromptWindow = true;

                if (freeProxy != null)
                {
                    service.ProxyType = freeProxy.Protocol == ProxyProtocolsEnum.HTTP ? "http" : "https:";
                    var proxy = new Proxy
                    {
                        HttpProxy = $"{freeProxy.IPAddress}:{freeProxy.PortNo}",
                    };
                    service.Proxy = proxy.HttpProxy;
                }

                _driver = new PhantomJSDriver(service)
                {
                    Url = hostPgUrlPattern
                };

                _driver.Navigate();

                // The driver can now provide you with what we 
                // need (it will execute the script) 
                // get the source of the page
                var content = _driver.PageSource;
                _driver.Quit();

                return content;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        public void Dispose()
        {
            if (_driver != null)
                _driver.Quit();
        }
    }
}
