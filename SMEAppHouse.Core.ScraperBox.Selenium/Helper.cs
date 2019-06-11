using OpenQA.Selenium.PhantomJS;
using System;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;

#pragma warning disable 618

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
        /// <param name="targetPgUrl"></param>
        /// <param name="proxy"></param>
        /// <returns></returns>
        public static string GrabPage(string targetPgUrl, Tuple<string, string> proxy = null)
        {
            //var options = new ChromeOptions();
            //var userAgent = "user_agent_string";
            //options.AddArgument("--user-agent=" + userAgent);
            //IWebDriver driver = new ChromeDriver(options);

            IWebDriver driver = null;
            Exception exception = null;
            var content = string.Empty;

            try
            {
                var service = PhantomJSDriverService.CreateDefaultService(".\\");
                service.HideCommandPromptWindow = true;

                if (proxy != null)
                    service.Proxy = $"{proxy.Item1}:{proxy.Item2}";

                driver = new PhantomJSDriver(service)
                {
                    Url = targetPgUrl
                };

                content = GrabPage((PhantomJSDriver)driver, targetPgUrl, proxy);

                return content;
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            finally
            {
                if (driver != null)
                {
                    Task.Delay(2000).ContinueWith( t =>
                    {
                        Thread.Sleep(1000);
                        driver.Quit();
                        driver.Dispose();
                    });
                }

                if (exception != null)
                    throw exception;
            }

            return content;
        }

        public static string GrabPage(PhantomJSDriver driver, string targetPgUrl, Tuple<string, string> proxy = null)
        {
            if (driver == null)
                throw new InvalidOperationException("driver was not supplied");

            if (proxy != null)
            {
                var script = $"return phantom.setProxy(\"{proxy.Item1}\", {proxy.Item2}, \"http\", \"\", \"";
                var obj = driver?.ExecutePhantomJS(script);
            }

            driver.Url = targetPgUrl;
            driver.Navigate();
            return driver?.PageSource;
        }

    }
}
