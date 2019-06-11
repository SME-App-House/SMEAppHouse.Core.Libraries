using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using SMEAppHouse.Core.ScraperBox.Models;
using System;
using static SMEAppHouse.Core.ScraperBox.Models.PageInstruction;

namespace SMEAppHouse.Core.ScraperBox.Selenium
{
    public static class Helper
    {
        /// <summary>
        /// REFERENCES:
        /// other approach: 
        /// 
        ///     http://stackoverflow.com/questions/18921099/add-proxy-to-phantomjsdriver-selenium-c
        ///         PhantomJSOptions phoptions = new PhantomJSOptions();
        ///         phoptions.AddAdditionalCapability(CapabilityType.Proxy, "http://localhost:9999");
        ///         
        ///     driver = new PhantomJSDriver(phoptions);
        ///     - (24/04/2019) https://stackoverflow.com/a/52446115/3796898
        ///     
        /// </summary>
        /// <param name="hostPgUrlPattern"></param>
        /// <param name="freeProxy"></param>
        /// <returns></returns>
        public static string LoadIPProxyDocumentContent(string hostPgUrlPattern, IPProxy freeProxy = null)
        {
            //var options = new ChromeOptions();
            //var userAgent = "user_agent_string";
            //options.AddArgument("--user-agent=" + userAgent);
            //IWebDriver driver = new ChromeDriver(options);

            var service = PhantomJSDriverService.CreateDefaultService(".\\");
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

            var driver = new PhantomJSDriver(service)
            {
                Url = hostPgUrlPattern
            };

            driver.Navigate();
            var content = driver.PageSource;
            driver.Quit();

            return content;
        }
    }
}
