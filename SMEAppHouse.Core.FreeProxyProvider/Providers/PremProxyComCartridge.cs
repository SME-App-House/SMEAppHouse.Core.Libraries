using System;
using System.Linq;
using HtmlAgilityPack;
using SMEAppHouse.Core.FreeIPProxy.Providers.Base;
using SMEAppHouse.Core.ScraperBox;
using SMEAppHouse.Core.ScraperBox.Models;
using static SMEAppHouse.Core.ScraperBox.Models.PageInstruction;

#pragma warning disable 168

namespace SMEAppHouse.Core.FreeIPProxy.Providers
{
    public class PremProxyComCartridge : IPProxyCartridgeBase
    {
        public PremProxyComCartridge(int startPageNo = 0)
            : base("https://premproxy.com/list/{PAGENO}.htm",
                  new PageInstruction()
                  {
                      PadCharacter = '0',
                      PadLength = 2,
                      PaddingDirection = PaddingDirectionsEnum.ToLeft
                  },
                  startPageNo)
        {

        }

        protected override void ValidatePage(string content)
        {
            try
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(content);
                var document = doc.DocumentNode;
                var target = ScraperBox.Helper.GetNodeByAttribute(document, "table", "id", "proxylistt");
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
            var target = ScraperBox.Helper.GetNodeByAttribute(document, "table", "id", "proxylist");

            if (target == null) return;

            var lines = ScraperBox.Helper.GetNodeCollection(target, "tr");

            lines.ToList().ForEach(e =>
            {
                if (e.Descendants("td").Count() <= 1) return;

                var cells = e.Descendants("td").ToArray();
                var unwanted = cells[0].Descendants("script");
                var enumerable = unwanted as HtmlNode[] ?? unwanted.ToArray();
                if (enumerable.ToArray().Any())
                    enumerable.ToArray()[0].Remove();

                var proxy = new IPProxy()
                {
                    ProviderId = GetType().Name.Replace("Cartridge", ""),
                    //Valid = true
                };

                // get address's part
                try
                {
                    var address = cells[0];
                    var addressParts = address.InnerText.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    var ipAddress = addressParts[1].Replace("port", "").Trim();
                    var portNo = int.Parse(addressParts[2].Trim());
                    proxy.IPAddress = ipAddress;
                    proxy.PortNo = portNo;
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    //throw;
                }
                
                // get anonymity level
                proxy.AnonymityLevel = cells[1].InnerText.Trim().Contains("anonymous")
                    ? IPProxyRules.ProxyAnonymityLevelsEnum.Anonymous
                    : IPProxyRules.ProxyAnonymityLevelsEnum.Elite;

                // get last checked time : Apr-27, 16:22
                var checkdateTxt = cells[2].InnerText.Replace("Checked:", "").Trim();
                var checkdateParts = checkdateTxt.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                var checkdateCalPrts = checkdateParts[0].Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                //"2009 Apr 8 14:40:52,531 <--> yyyy-MM-dd HH:mm:ss,fff
                var checkDate = $"{checkdateCalPrts[1]} {checkdateCalPrts[0]} {DateTime.Now.Year}";
                var checkDatePrsd = DateTime.ParseExact(checkDate, "dd MMM yyyy", System.Globalization.CultureInfo.InvariantCulture);
                proxy.LastChecked = checkDatePrsd.Add(TimeSpan.Parse(checkdateParts[1]));

                //todo:
                //proxy.LastValidationCheck = DateTime.Parse(checkdate);

                // get the country
                var countryPartial = cells[3].InnerText
                    .Replace("Country:", "")
                    .Trim().Replace(" ", "_")
                    .ToLower();

                if (!string.IsNullOrEmpty(countryPartial))
                {
                    countryPartial = countryPartial.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)[0];
                    proxy.Country = ScraperBox.Helper.FindProxyCountryFromPartial(countryPartial);
                }

                proxy.City = cells[4].InnerText.Replace("City:", "").Replace("&nbsp;", "").Trim();
                proxy.ISP = cells[5].InnerText.Replace("ISP:", "");

                //if (!ProxyTestHelper.CanPing(string.Format("{0}://{1}:{2}", proxy.Protocol == ProxyProtocolEnum.HTTP ? "http" : "https", proxy.HostIP, proxy.PortNo)))
                //if (!ProxyTestHelper.ProxyIsGood(proxy.HostIP, proxy.PortNo)) return;

                RegisterProxy(proxy);
            });

            base.ParseProxyPage(content);
        }


    }
}
