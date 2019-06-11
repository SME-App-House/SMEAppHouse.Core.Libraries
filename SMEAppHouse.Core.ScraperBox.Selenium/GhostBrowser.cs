using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using SMEAppHouse.Core.ScraperBox.Models;
#pragma warning disable 618

namespace SMEAppHouse.Core.ScraperBox.Selenium
{
    public class GhostBrowser : IDisposable
    {
        public IWebDriver WebDriver { get; set; }

        public GhostBrowser()
        {
            var service = PhantomJSDriverService.CreateDefaultService(".\\");
            service.HideCommandPromptWindow = true;

            this.WebDriver = new PhantomJSDriver(service);
        }

        public void UpdateProxy(IPProxy proxy)
        {
            if (proxy != null)
            {
                var script = $"return phantom.setProxy(\"{proxy.IPAddress}\", {proxy.PortNo}, \"http\", \"\", \"";
                var obj = (this.WebDriver as PhantomJSDriver)?.ExecutePhantomJS(script);
            }
        }

        public void Dispose()
        {
            Task.Delay(1000).ContinueWith(t =>
            {
                Thread.Sleep(1000);
                this.WebDriver.Quit();
                this.WebDriver.Dispose();
            });
        }
    }
}
