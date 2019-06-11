using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using SMEAppHouse.Core.FreeIPProxy.Models;
using SMEAppHouse.Core.FreeIPProxy.Providers.Base;
using SMEAppHouse.Core.ScraperBox;

namespace SMEAppHouse.Core.FreeIPProxy.Providers
{
    public class FromProxyHttpNet : IPProxyProviderBase
    {
        private new const string HostPageUrlPattern = "http://proxyhttp.net/free-list/anonymous-server-hide-ip-address/{PAGENO}#proxylist";
        private new const int HostPageMax = 100;

        public FromProxyHttpNet(bool consumeSelf, bool autoStart)
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
                var target = HtmlUtil.GetNodeByAttribute(document, "table", "class", "proxytbl");

                if (target == null) return null;

                var lines = HtmlUtil.GetNodeCollection(target, "tr");

                lines.ToList().ForEach(e =>
                {
                    if (freeProxies == null)
                        freeProxies = new Queue<FreeProxy>();

                    if (e.Descendants("td").Count() <= 1) return;

                    var proxy = new FreeProxy();
                    var cells = e.Descendants("td").ToArray();

                    //ip
                    var ip = cells[0].InnerText;

                    //port
                    var scriptPart = cells[1].Descendants("script");
                    var htmlNodes = scriptPart as HtmlNode[] ?? scriptPart.ToArray();
                    if (htmlNodes.Any())
                        htmlNodes.ToArray()[0].Remove();
                    var port = HtmlUtil.Resolve(cells[1].InnerText);

                    // country
                    var country = HtmlUtil.Resolve(cells[2].InnerText);

                    // anon
                    var anon = HtmlUtil.Resolve(cells[3].InnerText);

                    //https
                    var http = HtmlUtil.Resolve(cells[4].Attributes["class"].Value);


                    //last check
                    var lastChecked = HtmlUtil.Resolve(cells[5].InnerText);

                    freeProxies.Enqueue(proxy);
                    freeProxyFetched(proxy);
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
