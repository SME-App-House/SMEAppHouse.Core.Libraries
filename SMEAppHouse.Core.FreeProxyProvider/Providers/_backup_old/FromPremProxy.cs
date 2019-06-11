using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using SMEAppHouse.Core.FreeIPProxy.Providers.Base;
using SMEAppHouse.Core.FreeIPProxy.Models;
using SMEAppHouse.Core.ScraperBox;

namespace SMEAppHouse.Core.FreeIPProxy.Providers
{
    public class FromPremProxy : IPProxyProviderBase
    {
        private new const string HostPageUrlPattern = "https://premproxy.com/list/{PAGENO}.htm";
        private new const int HostPageMax = 100;


        public FromPremProxy(bool consumeSelf)
            : this(consumeSelf, false)
        {
        }

        public FromPremProxy(bool consumeSelf, bool autoStart)
            : base(consumeSelf)
        {
            base.HostPageMax = HostPageMax;
            base.HostPageUrlPattern = HostPageUrlPattern;

            if (autoStart)
                Start();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageContentDocument"></param>
        /// <param name="freeProxyFetched"></param>
        /// <returns></returns>
        public override Queue<FreeProxy> LoadUpIPProxies(string pageContentDocument, Action<FreeProxy> freeProxyFetched)
        {
            Queue<FreeProxy> freeProxies = null;

            try
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(pageContentDocument);
                var document = doc.DocumentNode;
                var target = HtmlUtil.GetNodeByAttribute(document, "table", "id", "proxylist");

                if (target == null) return null;

                var lines = HtmlUtil.GetNodeCollection(target, "tr");

                lines.ToList().ForEach(e =>
                {
                    if (freeProxies == null)
                        freeProxies = new Queue<FreeProxy>();

                    if (e.Descendants("td").Count() <= 1) return;

                    try
                    {
                        var proxy = new FreeProxy();
                        var cells = e.Descendants("td").ToArray();

                        var unwanted = cells[0].Descendants("script");
                        var enumerable = unwanted as HtmlNode[] ?? unwanted.ToArray();

                        if (unwanted != null && enumerable.ToArray().Any())
                            enumerable.ToArray()[0].Remove();

                        var address = cells[0];

                        var addressParts = address.InnerText.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                        proxy.IPAddress = addressParts[0];
                        proxy.PortNo = int.Parse(addressParts[1]);

                        // get anonymity level
                        //proxy.AnonymityLevel = cells[1].InnerText.Contains("high")
                        //    ? ProxyAnonymityLevelEnum.High
                        //    : ProxyAnonymityLevelEnum.Medium;

                        proxy.AnonymityLevel = cells[1].InnerText;

                        // get last checked time
                        var checkdate = cells[2].InnerText;

                        //todo:
                        //proxy.LastValidationCheck = DateTime.Parse(checkdate);

                        // get the country

                        var countryPartial = cells[3].InnerText.ToLower().Replace(" ", "_");
                        countryPartial = countryPartial.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[0];
                        var pxycountry = FindProxyCountryFromPartial(countryPartial);
                        proxy.Country = pxycountry;

                        //if (!ProxyTestHelper.CanPing(string.Format("{0}://{1}:{2}", proxy.Protocol == ProxyProtocolEnum.HTTP ? "http" : "https", proxy.HostIP, proxy.PortNo)))

                        //if (!ProxyTestHelper.ProxyIsGood(proxy.HostIP, proxy.PortNo)) return;

                        freeProxies.Enqueue(proxy);
                        freeProxyFetched(proxy);
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }

                });
            }
            catch (Exception ex)
            {
                throw;
            }

            return freeProxies;
        }
    }
}
