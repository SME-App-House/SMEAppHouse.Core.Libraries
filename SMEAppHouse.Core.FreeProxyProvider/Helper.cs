using System;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using SMEAppHouse.Core.ScraperBox;
using SMEAppHouse.Core.ScraperBox.Models;
using SeleniumHelper = SMEAppHouse.Core.ScraperBox.Selenium.Helper;

namespace SMEAppHouse.Core.FreeIPProxy
{
    public static class Helper
    {
        //public static bool TestIPProxy(IPProxy proxy, Tuple<string, string> webSrvrCredential = null)
        //{
        //    if (proxy.Protocol != ProxyProtocolsEnum.HTTP)
        //    {
        //        proxy.CheckStatus = IPProxy.CheckStatusEnum.CheckedInvalid;
        //        proxy.LastChecked = DateTime.Now;
        //        return false;
        //    }

        //    var protHt = proxy.Protocol == ProxyProtocolsEnum.HTTPS ? "https://" : proxy.Protocol == ProxyProtocolsEnum.HTTP ? "http://" : "";
        //    var prxy = new WebProxy()
        //    {
        //        Address = new Uri($"{protHt}www.{proxy.IPAddress}:{proxy.PortNo}"),
        //        BypassProxyOnLocal = true,
        //        UseDefaultCredentials = false,
        //    };

        //    if (proxy.Credential != null)
        //        // *** These credentials are given to the proxy server, not the web server ***
        //        prxy.Credentials = proxy.ToNetworkCredential();

        //    // Now create a client handler which uses that proxy
        //    var httpClientHandler = new HttpClientHandler()
        //    {
        //        UseProxy = true,
        //        Proxy = prxy,
        //        DefaultProxyCredentials = CredentialCache.DefaultNetworkCredentials
        //    };

        //    // Omit this part if you don't need to authenticate with the web server:
        //    if (webSrvrCredential != null)
        //    {
        //        httpClientHandler.PreAuthenticate = true;
        //        httpClientHandler.UseDefaultCredentials = false;

        //        // *** These credentials are given to the web server, not the proxy server ***
        //        httpClientHandler.Credentials = new NetworkCredential(
        //            userName: webSrvrCredential.Item1,
        //            password: webSrvrCredential.Item2);
        //    }

        //    // Finally, create the HTTP client object to test the proxy
        //    using (var client = new HttpClient(handler: httpClientHandler, disposeHandler: true))
        //    {
        //        var sw = Stopwatch.StartNew();
        //        proxy.CheckStatus = IPProxy.CheckStatusEnum.Checking;

        //        try
        //        {
        //            var task = client.GetStringAsync("http://example.com");
        //            var response = task.Wait(10000);
        //            proxy.CheckStatus = IPProxy.CheckStatusEnum.Checked;
        //            proxy.SpeedRate = (int)sw.ElapsedMilliseconds;
        //        }
        //        catch (Exception e)
        //        {
        //            proxy.CheckStatus = IPProxy.CheckStatusEnum.CheckedInvalid;
        //        }
        //        finally
        //        {
        //            sw.Stop();
        //            proxy.LastChecked = DateTime.Now;
        //        }
        //    }

        //    return proxy.CheckStatus == IPProxy.CheckStatusEnum.Checked;
        //}

        //public static bool TestIPProxy(IPProxy proxy)
        //{
        //    //if (proxy.Protocol != ProxyProtocolsEnum.HTTP)
        //    //{
        //    //    proxy.CheckStatus = IPProxy.CheckStatusEnum.CheckedInvalid;
        //    //    proxy.LastChecked = DateTime.Now;
        //    //    return false;
        //    //}

        //    var protHt = proxy.Protocol == ProxyProtocolsEnum.HTTPS ? "https://" : proxy.Protocol == ProxyProtocolsEnum.HTTP ? "http://" : "";
        //    var prxy = new WebProxy()
        //    {
        //        Address = new Uri($"{protHt}{proxy.IPAddress}:{proxy.PortNo}"),
        //        Credentials = CredentialCache.DefaultCredentials,

        //        //still use the proxy for local addresses
        //        BypassProxyOnLocal = false,

        //    };

        //    if (proxy.Credential != null)
        //    {
        //        prxy.UseDefaultCredentials = false;
        //        // *** These credentials are given to the proxy server, not the web server ***
        //        prxy.Credentials = proxy.ToNetworkCredential();
        //    }

        //    var sw = Stopwatch.StartNew();

        //    // Finally, create the HTTP client object to test the proxy
        //    var request = (HttpWebRequest)WebRequest.Create("http://example.com/");
        //    request.Proxy = prxy;

        //    try
        //    {
        //        proxy.CheckStatus = IPProxy.CheckStatusEnum.Checking;
        //        using (var req = request.GetResponse())
        //        {
        //            using (var reader = new StreamReader(req.GetResponseStream()))
        //            {
        //                var response = reader.ReadToEnd();
        //                proxy.CheckStatus = IPProxy.CheckStatusEnum.Checked;
        //                proxy.SpeedRate = (int)sw.ElapsedMilliseconds;
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        proxy.CheckStatus = IPProxy.CheckStatusEnum.CheckedInvalid;
        //        proxy.SpeedRate = 0;
        //    }
        //    finally
        //    {
        //        sw.Stop();
        //        proxy.LastChecked = DateTime.Now;
        //    }

        //    return proxy.CheckStatus == IPProxy.CheckStatusEnum.Checked;
        //}

        public static bool ValidateProxy(string host, int port)
        {
            try
            {
                var wc = new WebClient { Proxy = new WebProxy(host, port) };
                wc.DownloadString("http://google.com/ncr");
                return true;
            }
            catch (Exception ex)
            {
                // ignored
            }
            return false;
        }

        public static bool InternetIsGood()
        {
            try
            {
                var wc = new WebClient();
                wc.DownloadString("http://google.com/ncr");
                return true;
            }
            catch
            {
                // ignored
            }
            return false;
        }

        /// <summary>
        /// Test if proxy is working or not by pinging directly on the host
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static bool ValidateProxyByPing(string host, int port)
        {
            var address = $"{host}:{port}";
            return ValidateProxyByPing(address);

        }

        /// <summary>
        /// Test if proxy is working or not by pinging directly in the host
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static bool ValidateProxyByPing(string address)
        {
            var ping = new Ping();
            try
            {
                var reply = ping.Send(address, 2000);
                if (reply == null) return false;
                return (reply.Status == IPStatus.Success);
            }
            catch (PingException e)
            {
                return false;
            }
        }

        public static bool CanPing(string address)
        {
            Ping ping = new Ping();

            try
            {
                PingReply reply = ping.Send(address, 2000);
                if (reply == null) return false;

                return (reply.Status == IPStatus.Success);
            }
            catch (PingException e)
            {
                return false;
            }
        }


        /// <summary>
        /// Test if socket proxy is working or not
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static bool ValidateSocket(string host, int port)
        {
            var isSuccess = false;
            try
            {
                var connsock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                connsock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 200);
                Thread.Sleep(500);
                var hip = IPAddress.Parse(host);
                var ipep = new IPEndPoint(hip, port);
                connsock.Connect(ipep);
                isSuccess = connsock.Connected;
                connsock.Close();
            }
            catch (Exception ex)
            {
                isSuccess = false;
            }
            return isSuccess;
        }

        public static bool TestIPProxy2(IPProxy proxy)
        {
            try
            {
                var sw = Stopwatch.StartNew();
                proxy.CheckStatus = IPProxy.CheckStatusEnum.Checking;
                var content = SeleniumHelper.GrabPage("http://example.com", proxy.AsTuple());
                sw.Stop();
                proxy.SpeedRate = (int) sw.ElapsedMilliseconds;
                proxy.CheckStatus = IPProxy.CheckStatusEnum.Checked;
            }
            catch (Exception e)
            {
                proxy.CheckStatus = IPProxy.CheckStatusEnum.CheckedInvalid;
            }
            return proxy.CheckStatus == IPProxy.CheckStatusEnum.Checked;
        }

        public static bool TestIPProxy(IPProxy proxy)
        {
            var protHt = proxy.Protocol == IPProxyRules.ProxyProtocolsEnum.HTTPS ? "https://" : proxy.Protocol == IPProxyRules.ProxyProtocolsEnum.HTTP ? "http://" : "";
            var prxy = new WebProxy()
            {
                Address = new Uri($"{protHt}{proxy.IPAddress}:{proxy.PortNo}"),
                Credentials = CredentialCache.DefaultCredentials,

                //still use the proxy for local addresses
                BypassProxyOnLocal = false,

            };

            if (proxy.Credential != null)
            {
                prxy.UseDefaultCredentials = false;
                // *** These credentials are given to the proxy server, not the web server ***
                prxy.Credentials = proxy.ToNetworkCredential();
            }

            var sw = Stopwatch.StartNew();

            // Finally, create the HTTP client object to test the proxy
            var request = (HttpWebRequest)WebRequest.Create("http://example.com/");
            request.Proxy = prxy;

            try
            {
                proxy.CheckStatus = IPProxy.CheckStatusEnum.Checking;
                using (var req = request.GetResponse())
                {
                    using (var reader = new StreamReader(req.GetResponseStream()))
                    {
                        var response = reader.ReadToEnd();
                        proxy.CheckStatus = IPProxy.CheckStatusEnum.Checked;
                        proxy.SpeedRate = (int)sw.ElapsedMilliseconds;
                    }
                }
            }
            catch (Exception e)
            {
                proxy.CheckStatus = IPProxy.CheckStatusEnum.CheckedInvalid;
                proxy.SpeedRate = 0;
            }
            finally
            {
                sw.Stop();
                proxy.LastChecked = DateTime.Now;
            }

            return proxy.CheckStatus == IPProxy.CheckStatusEnum.Checked;
        }

    }
}
