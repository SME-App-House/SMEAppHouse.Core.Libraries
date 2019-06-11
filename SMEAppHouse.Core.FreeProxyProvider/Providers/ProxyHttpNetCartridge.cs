using System;
using System.Linq;
using HtmlAgilityPack;
using SMEAppHouse.Core.FreeIPProxy.Providers.Base;
using SMEAppHouse.Core.ScraperBox;
using SMEAppHouse.Core.ScraperBox.Models;

// ReSharper disable EmptyGeneralCatchClause
#pragma warning disable 168

namespace SMEAppHouse.Core.FreeIPProxy.Providers
{
    public class ProxyHttpNetCartridge : IPProxyCartridgeBase
    {
        public ProxyHttpNetCartridge(int startPageNo = 0)
            : base("http://proxyhttp.net/free-list/anonymous-server-hide-ip-address/{PAGENO}#proxylist",
                  startPageNo)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        protected override void ValidatePage(string content)
        {
            try
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(content);
                var document = doc.DocumentNode;
                var target = ScraperBox.Helper.GetNodeByAttribute(document, "table", "class", "proxytbl");

                PageIsValid = target != null;

            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        protected override void ParseProxyPage(string content)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(content);
            var document = doc.DocumentNode;
            var target = ScraperBox.Helper.GetNodeByAttribute(document, "table", "class", "proxytbl");

            if (target == null) return;

            var lines = ScraperBox.Helper.GetNodeCollection(target, "tr");

            lines.ToList().ForEach(e =>
            {
                if (e.Descendants("td").Count() <= 1) return;

                var proxy = new IPProxy()
                {
                    ProviderId = GetType().Name.Replace("Cartridge", ""),
                };
                var cells = e.Descendants("td").ToArray();

                var scriptPart = cells[1].Descendants("script");
                var htmlNodes = scriptPart as HtmlNode[] ?? scriptPart.ToArray();
                if (htmlNodes.Any())
                    htmlNodes.ToArray()[0].Remove();

                //ip
                proxy.IPAddress = cells[0].InnerText.Trim();

                //port
                proxy.PortNo = int.Parse(ScraperBox.Helper.Resolve(cells[1].InnerText.Trim()).Replace("\r\n", "").Trim());

                // country
                var country = ScraperBox.Helper.Resolve(cells[2].InnerText.Trim());
                var countryPrts = country.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                proxy.Country = ScraperBox.Helper.FindProxyCountryFromPartial(countryPrts[0].Replace(" ", "_"));

                // anon
                proxy.AnonymityLevel = cells[3].InnerText.Trim().Replace("\r\n", "").Trim().Contains("anonymous")
                    ? IPProxyRules.ProxyAnonymityLevelsEnum.Anonymous
                    : IPProxyRules.ProxyAnonymityLevelsEnum.Elite;

                //protocol
                var https = ScraperBox.Helper.Resolve(cells[4].Attributes["class"].Value);
                proxy.Protocol = https.ToLower().Contains("https")
                    ? IPProxyRules.ProxyProtocolsEnum.HTTPS
                    : IPProxyRules.ProxyProtocolsEnum.HTTP;

                //last check
                var lastChecked = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
                proxy.LastChecked = lastChecked.Add(TimeSpan.Parse(ScraperBox.Helper.Resolve(cells[5].InnerText.Trim())));

                RegisterProxy(proxy);

            });

            base.ParseProxyPage(content);
        }

    }
}
