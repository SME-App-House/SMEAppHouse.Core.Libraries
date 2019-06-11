using System;
using System.Net;
using System.Threading.Tasks;
using PuppeteerSharp;

namespace SMEAppHouse.Core.PuppeteerAdapter
{
    public interface IPuppeteerCrawlerAgent : IDisposable
    {
        Browser Browser { get; set; }
        Page HtmlPage { get; set; }

        bool IsReady { get; }
        bool IsHeadless { get; }

        bool TryGrabPage(string url, out Task<Response> responseTsk, bool inNewPage = false, bool? noImage = false);
        bool TryGrabPage(string url, out Task<Response> responseTsk, out Page page, bool inNewPage = false, bool? noImage = false);
        Task<T> ExecuteJava<T>(string jsCode) where T : struct;
        Task PerformRetry<TException>(Func<Task> operation, int maxRetryAttempts = 3) where TException : Exception;
        Task<string> GetImageAsBase64Url(string url, NetworkCredential credentials = null);
    }
}