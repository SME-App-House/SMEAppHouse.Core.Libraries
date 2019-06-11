using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using PuppeteerSharp;
using SMEAppHouse.Core.PuppeteerAdapter.Helpers;

namespace SMEAppHouse.Core.PuppeteerAdapter
{
    public class PuppeteerCrawlerAgent : IPuppeteerCrawlerAgent
    {
        public Browser Browser { get; set; }
        public Page HtmlPage { get; set; }

        public bool IsReady { get; private set; }
        public bool IsHeadless { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="headless"></param>
        /// <param name="noSandbox"></param>
        /// <param name="no2DCanvas"></param>
        /// <param name="noGPU"></param>
        public PuppeteerCrawlerAgent(bool headless = true, bool? noSandbox = null, bool? no2DCanvas = null, bool? noGPU = null)
        {
            IsHeadless = headless;

            Task.Factory.StartNew(async () =>
            {
                Console.WriteLine("Attempting to update the chromium engine browser to local...");

                await (new BrowserFetcher())
                    .DownloadAsync(BrowserFetcher.DefaultRevision);
                Console.WriteLine("Navigating to developers.google.com");

                // Args = new string[] { "--proxy-server='direct://'",
                //                       "--proxy-bypass-list=*" }

                var browserArgs = new List<string>();
                if (noSandbox.HasValue && noSandbox.Value) browserArgs.Add("--no-sandbox");
                if (no2DCanvas.HasValue && no2DCanvas.Value) browserArgs.Add("--disable-accelerated-2d-canvas");
                if (noGPU.HasValue && noGPU.Value) browserArgs.Add("--disable-gpu");

                Browser = await Puppeteer.LaunchAsync(new LaunchOptions
                {
                    Headless = IsHeadless,
                    Args = browserArgs.ToArray()
                });

                var pages = await Browser.PagesAsync();
                if (pages.FirstOrDefault() != null)
                    HtmlPage = pages.FirstOrDefault();
                else HtmlPage = await Browser.NewPageAsync();

                if (HtmlPage == null)
                    throw new Exception("Cannot create new page.");

                if (!IsHeadless)
                    await HtmlPage.SetViewportAsync(new ViewPortOptions() { Width = 1024, Height = 842 });

                IsReady = true;
            });
        }

        public bool TryGrabPage(string url, out Task<Response> responseTsk, bool inNewPage = false, bool? noImage = false)
        {
            return TryGrabPage(url, out responseTsk, out _, inNewPage, noImage);
        }

        public bool TryGrabPage(string url, out Task<Response> responseTsk, out Page page, bool inNewPage = false, bool? noImage = false)
        {
            if (!IsReady)
                throw new Exception("Chromium instance is not ready yet.");

            var pg = HtmlPage;

            if (inNewPage)
                Task.Run(async () =>
                {
                    pg = await Browser.NewPageAsync();
                    if (!IsHeadless)
                        await pg.SetViewportAsync(new ViewPortOptions() { Width = 1024, Height = 842 });
                }).Wait();

            Task.Run(async () =>
            {
                if (noImage.HasValue && noImage.Value)
                {
                    await pg.SetRequestInterceptionAsync(true);

                    // disable images to download
                    pg.Request += (sender, e) =>
                    {
                        if (e.Request.ResourceType == ResourceType.Image)
                            e.Request.AbortAsync().Wait();
                        else
                            e.Request.ContinueAsync().Wait();
                    };

                }
            }).Wait();


            var catContTsk = pg.GoToAsync(url, WaitUntilNavigation.Networkidle2);
            catContTsk.Wait();

            var jsCode = @"() => {
const selector = document.querySelector('[class=""error-404""]'); 
return selector ? (selector.innerHTML || '') : '';
}";
            var checkErrorTsk = HtmlPage.EvaluateFunctionAsync<string>(jsCode);
            var checkError = checkErrorTsk.Result;
            responseTsk = catContTsk;
            page = pg;

            if (!noImage.HasValue || !noImage.Value)
                return string.IsNullOrEmpty(checkError);

            pg.SetRequestInterceptionAsync(false).Wait();
            pg.Request -= null;

            return string.IsNullOrEmpty(checkError);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsCode"></param>
        /// <returns></returns>
        public async Task<T> ExecuteJava<T>(string jsCode) where T : struct
        {
            return await HtmlPage.EvaluateFunctionAsync<T>(jsCode);

            // TODO: add await selector here!
        }

        public void Dispose()
        {
            Task.Factory.StartNew(async () =>
            {
                await HtmlPage.CloseAsync();
                await Browser.CloseAsync();
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TException">e.g. HttpRequestException</typeparam>
        /// <param name="operation"></param>
        /// <param name="maxRetryAttempts"></param>
        /// <returns></returns>
        public async Task PerformRetry<TException>(Func<Task> operation, int maxRetryAttempts = 3) where TException : Exception
        {
            var pauseBetweenFailures = TimeSpan.FromSeconds(2);
            await RetryHelper.RetryOnExceptionAsync<TException>(maxRetryAttempts, pauseBetweenFailures, operation);
        }

        /// <summary>
        /// Ffetch the data from the URL as binary data and convert that to base64. This assumes the image
        /// always will be a JPEG. If it could sometimes be a different content type, you may well want to
        /// fetch the response as an HttpResponse and use that to propagate the content type.
        /// For future improvement, we may also want to add caching here as well.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="credentials"></param>
        /// <returns></returns>
        public async Task<string> GetImageAsBase64Url(string url, NetworkCredential credentials = null)
        {
            using (var handler = new HttpClientHandler { Credentials = credentials })
            using (var client = new HttpClient(handler))
            {
                var bytes = await client.GetByteArrayAsync(url);
                return "image/jpeg;base64," + Convert.ToBase64String(bytes);
            }
        }

    }
}
