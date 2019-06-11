using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using SMEAppHouse.Core.FreeProxyProvider.Handlers;
using SMEAppHouse.Core.FreeProxyProvider.Helpers;
using SMEAppHouse.Core.FreeProxyProvider.Models;
using GPS.Frameworks.HtmlHelper;
using HtmlAgilityPack;

namespace SMEAppHouse.Core.FreeProxyProvider.Providers.HideMyAss
{
    public class FreeIPGenerator2
    {
        private const string HMAProxListUrl = "http://hidemyass.com/proxy-list/{0}";

        private readonly object _proxyProviderLocker = new object();
        private readonly object _proxyValidatorLocker = new object();
        private readonly ProxyProtocolEnum[] _protocolOptions;

        private volatile bool _resume;
        private volatile bool _reset;
        private volatile bool _stopExtraction;
        private volatile bool _reachedLastPage;

        public ProxyProtocolEnum? Protocol { get; private set; }
        public ProxyAnonymityLevelEnum? AnonymityLevel { get; private set; }
        public ProxySpeedEnum? Speed { get; private set; }
        public ProxyConnectionSpeedEnum? ConnectionTime { get; private set; }

        public Guid RotationId { get; private set; }
        public List<FreeProxy> FreeProxies { get; set; }
        public bool IsActive { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public FreeIPGenerator2()
            : this(null, null, null, null)
        {
        }
        public FreeIPGenerator2(ProxyProtocolEnum[] proxyProtocolOptions,
                                ProxyAnonymityLevelEnum anonymityLevel,
                                ProxyConnectionSpeedEnum connectionTime,
                                ProxySpeedEnum speed)
            : this(null, anonymityLevel, speed, connectionTime)
        {
            _protocolOptions = proxyProtocolOptions;
        }
        public FreeIPGenerator2(ProxyProtocolEnum? protocol,
                                ProxyAnonymityLevelEnum? anonymityLevel,
                                ProxySpeedEnum? speed,
                                ProxyConnectionSpeedEnum? connectionTime)
        {

            if (FreeProxies == null) FreeProxies = new List<FreeProxy>();

            if (protocol.HasValue) Protocol = protocol.Value;
            if (anonymityLevel.HasValue) AnonymityLevel = anonymityLevel.Value;
            if (speed.HasValue) Speed = speed.Value;
            if (connectionTime.HasValue) ConnectionTime = connectionTime.Value;

            new Task(RunProxyProviderThread)
            .Start();

            new Task(RunProxyValidationThread)
            .Start();
        }

        /// <summary>
        /// FreeIPGeneratorException
        /// </summary>
        private void RunProxyProviderThread()
        {
            lock (_proxyProviderLocker)
            {
                var hmaPgIdx = 1;
                var pgEnding = false;

                //this.RotationId = Guid.NewGuid();

                do
                {
                    if (_reachedLastPage | _reset)
                    {
                        hmaPgIdx = 1;
                        pgEnding = false;
                        _reset = false;
                    }

                    while (!pgEnding && !_stopExtraction)
                    {
                        if (_resume)
                        {
                            if (hmaPgIdx == 1 && OnBeginningOfProxyListPage != null)
                                OnBeginningOfProxyListPage(this, new EventArgs());

                            var hmaProxListUrl = string.Format(HMAProxListUrl, hmaPgIdx);

                            try
                            {
                                var fPxy = GetFreeProxy(true);
                                var hmaPgDoc = (fPxy != null)
                                    ? HtmlUtil.GetPageDocument(new Uri(hmaProxListUrl), fPxy.ToWebProxy(), UserAgents.GetFakeUserAgent(UserAgents.Chrome41022280))
                                    : HtmlUtil.GetPageDocument(hmaProxListUrl, UserAgents.GetFakeUserAgent(UserAgents.Chrome41022280));

                                hmaPgDoc = hmaPgDoc.Replace(Environment.NewLine, "");

                                var start = hmaPgDoc.IndexOf("<table id=\"listtable\"", StringComparison.Ordinal);
                                var end = hmaPgDoc.IndexOf("<div id=\"pagination\">", StringComparison.Ordinal);
                                hmaPgDoc = hmaPgDoc.Substring(start, end - start);

                                var results = ExtractProxies(hmaPgDoc);

                                if (_reachedLastPage)
                                {
                                    hmaPgIdx = 1;
                                }
                                else
                                    hmaPgIdx++;

                                if (results == 1 | _reachedLastPage)
                                {
                                    hmaPgIdx = hmaPgIdx--; // go back one page in index
                                    pgEnding = true;

                                    _reachedLastPage = true;

                                    if (OnEndOfProxyListPage != null && !_reachedLastPage)
                                        OnEndOfProxyListPage(this, new EventArgs());
                                }

                            }
                            catch (Exception ex)
                            {
                                var errMsgPrefx = string.Format("Error@{0}/pgIdx:[{1}]/errmsg:[{2}]", "RunIPProviderThread", hmaPgIdx, ex.Message);
                                var exception = new Exception(errMsgPrefx, ex);
                                InvokeEventFreeIPGeneratorException(new EventHandlers.FreeIPGeneratorExceptionEventArgs(exception));
                            }
                        }
                        Thread.Sleep(1000 * 10);
                    }
                    Thread.Sleep(10);
                }
                while (true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void RunProxyValidationThread()
        {
            lock (_proxyValidatorLocker)
            {
                do
                {
                    try
                    {
                        if (FreeProxies != null && FreeProxies.Any())
                        {
                            var proxies = FreeProxies
                                                .ToArray()
                                                .Where(p => !p.Valid | p.GetLastValidationElapsedTime().TotalMinutes > 30);

                            var freeProxies = proxies as FreeProxy[] ?? proxies.ToArray();
                            if (freeProxies.ToArray().Any())
                            {
                                var proxy = freeProxies.ToArray()[0];
                                if (proxy != null)
                                {
                                    var prxIsValid = ProxyTestHelper.SocketConnect(proxy.HostIP, proxy.PortNo);
                                    proxy.Valid = prxIsValid;
                                    proxy.LastValidationCheck = DateTime.Now;
                                }
                                break;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    Thread.Sleep(100);
                }
                while (true);
            }
        }

        public void Reset()
        {
            _reset = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Resume()
        {
            _resume = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            _resume = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public FreeProxy GetFreeProxy(bool onlyValid = false)
        {
            FreeProxy exFreeProxy = null;
            var ok = false;

            if (FreeProxies == null)
                throw new Exception("FreeProxies is null");

            while (!ok)
            {
                try
                {
                    if (RotationId == Guid.Empty | (FreeProxies.Count(p => p.RotationTokenId != RotationId) == 0))
                        RotationId = Guid.NewGuid();

                    FreeProxy[] proxies = null;

                    proxies = onlyValid
                                ? FreeProxies.ToArray().Where(p => p.RotationTokenId != RotationId && p.Valid).ToArray()
                                : FreeProxies.ToArray().Where(p => p.RotationTokenId != RotationId).ToArray();

                    if (proxies.Any())
                    {
                        exFreeProxy = proxies[0];
                        if (exFreeProxy != null)
                            exFreeProxy.RotationTokenId = RotationId;
                    }

                    ok = true; // _exFreeProxy != null && (_exFreeProxy.Valid && onlyValid);
                }
                catch
                {
                    throw;
                }
            }

            return exFreeProxy;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlString"></param>
        /// <returns></returns>
        private List<string> CollectUnsanitizadTags(string htmlString)
        {
            const string pattern = "<style[^<]*</style>";
            List<string> unsanitizadTags = null;
            var rgx = new Regex(pattern);

            try
            {
                foreach (Match match in rgx.Matches(htmlString))
                {
                    if (unsanitizadTags == null) unsanitizadTags = new List<string>();
                    unsanitizadTags.Add(match.Value);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return unsanitizadTags;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeHtml"></param>
        /// <returns></returns>
        private string GetStyleIdString(string nodeHtml)
        {
            const string pattern = "<style[^<]*</style>";
            string styleStr;

            try
            {
                var rgx = new Regex(pattern);
                var v = rgx.Match(nodeHtml);
                styleStr = v.Groups[0].ToString();
                styleStr = styleStr.Replace(" ", "").Trim();
            }
            catch (Exception)
            {
                throw;
            }
            return styleStr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="styleIds"></param>
        /// <param name="toOmit"></param>
        /// <param name="toNotOmit"></param>
        private void AnalyzeHidingStyles(string styleIds, ref string[] toOmit, ref string[] toNotOmit)
        {
            var _toOmit = new List<string>();
            var _toNotOmit = new List<string>();

            var styleItms = styleIds
                                    .Replace("<style>", "")
                                    .Replace("</style>", "")
                                    .Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            if (styleItms.Length <= 0) return;

            foreach (var s in styleItms)
            {
                if (s.Contains("display:none"))
                    _toOmit.Add(s.Replace(".", "").Replace("{display:none}", ""));
                else if (s.Contains("display:inline"))
                    _toNotOmit.Add(s.Replace(".", "").Replace("{display:inline}", ""));
            }

            toOmit = _toOmit.ToArray();
            toNotOmit = _toNotOmit.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeHtml"></param>
        /// <returns></returns>
        private string SnatchIPAddress(string nodeHtml)
        {
            var styleIds = GetStyleIdString(nodeHtml);
            string[] toOmit = null;
            string[] toNotOmit = null;

            AnalyzeHidingStyles(styleIds, ref toOmit, ref toNotOmit);

            var doc = new HtmlDocument();
            doc.LoadHtml(nodeHtml.Replace(styleIds, "").Replace("<span></span>", ""));

            var nodes = doc.DocumentNode.Descendants().ToArray();
            var final = doc.DocumentNode.InnerHtml;

            for (var ctr = 1; ctr < nodes.Count(); ctr++)
            {
                try
                {
                    var node = nodes[ctr];
                    var test = node.OuterHtml;

                    if (node.Name.Equals("#text")) continue;

                    var replaceStr = string.Empty;
                    var className = string.Empty;
                    var style = string.Empty;

                    if (node.Attributes["style"] != null &&
                        node.Attributes["style"].Value.Replace(" ", "").Equals("display:none"))
                        replaceStr = "";
                    else if (node.Attributes["style"] != null &&
                             node.Attributes["style"].Value.Replace(" ", "").Equals("display:inline"))
                        replaceStr = node.InnerText;

                    else if (node.Attributes["class"] != null)
                    {
                        className = node.Attributes["class"].Value;

                        if (toOmit.Contains(className))
                            replaceStr = string.Empty;
                        else if (toNotOmit.Contains(className))
                            replaceStr = node.InnerText;
                        else
                            replaceStr = node.InnerText;
                    }
                    final = final.Replace(node.OuterHtml, replaceStr);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return final.Replace("<span>", "").Replace("</span>", "");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rowNode"></param>
        /// <param name="freeProxy"></param>
        /// <returns></returns>
        private bool ParseRowToProxy(HtmlNode rowNode, ref FreeProxy freeProxy)
        {
            try
            {
                var prxCells = rowNode.Descendants("td");

                var htmlNodes = prxCells as HtmlNode[] ?? prxCells.ToArray();
                if (prxCells != null && htmlNodes.Any())
                {
                    freeProxy = new FreeProxy();
                    freeProxy.HostIP = SnatchIPAddress(htmlNodes.ToArray()[1].InnerHtml);
                    freeProxy.PortNo = int.Parse(htmlNodes.ToArray()[2].InnerText);

                    var country = htmlNodes.ToArray()[3].InnerText.Trim().ToUpper();

                    country = country.Replace("; ", "___")
                                        .Replace(", ", "__")
                                        .Replace(" ", "_")
                                        .Replace("'", "____");

                    freeProxy.Country = (ProxyCountry)Enum.Parse(typeof(ProxyCountry), country);

                    var connspeedNode = htmlNodes.ToArray()[5];
                    connspeedNode = HtmlUtil.GetNodeByAttribute(connspeedNode, "div", "class", "speedbar connection_time");
                    connspeedNode = connspeedNode.Descendants("div").ToArray()[0];

                    var rate = connspeedNode.Attributes["style"].Value.Replace("width:", "").Replace("%", "");
                    freeProxy.ResponseRate = int.Parse(rate);

                    if (freeProxy.ResponseRate <= 35)
                        freeProxy.ConnectionTime = ProxyConnectionSpeedEnum.Slow;
                    else if (freeProxy.ResponseRate > 35 && freeProxy.ResponseRate <= 65)
                        freeProxy.ConnectionTime = ProxyConnectionSpeedEnum.Medium;
                    else if (freeProxy.ResponseRate > 65)
                        freeProxy.ConnectionTime = ProxyConnectionSpeedEnum.Medium;

                    var speedNode = htmlNodes.ToArray()[4];
                    speedNode = HtmlUtil.GetNodeByAttribute(speedNode, "div", "class", "speedbar response_time");
                    speedNode = speedNode.Descendants("div").ToArray()[0];
                    rate = speedNode.Attributes["style"].Value.Replace("width:", "").Replace("%", "");
                    freeProxy.SpeedRate = int.Parse(rate);

                    if (freeProxy.SpeedRate <= 35)
                        freeProxy.Speed = ProxySpeedEnum.Slow;
                    else if (freeProxy.SpeedRate > 35 && freeProxy.SpeedRate <= 65)
                        freeProxy.Speed = ProxySpeedEnum.Medium;
                    else if (freeProxy.SpeedRate > 65)
                        freeProxy.Speed = ProxySpeedEnum.Medium;


                    var protocol = htmlNodes.ToArray()[6].InnerText.Replace("/", "_");
                    freeProxy.Protocol = (ProxyProtocolEnum)Enum.Parse(typeof(ProxyProtocolEnum), protocol);

                    var anonymity = htmlNodes.ToArray()[7].InnerText.Replace(" +", "__");
                    freeProxy.AnonymityLevel = (ProxyAnonymityLevelEnum)Enum.Parse(typeof(ProxyAnonymityLevelEnum), anonymity);

                    freeProxy.Valid = true;

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hmaPgDoc"></param>
        private int ExtractProxies(string hmaPgDoc)
        {
            var rowCount = 0;

            var doc = new HtmlDocument();
            doc.LoadHtml(hmaPgDoc);

            var tblNode = HtmlUtil.GetNodeByAttribute(doc.DocumentNode, "table", "id", "listtable");

            if (tblNode == null) return rowCount;

            var tRowNodes = tblNode.Descendants("tr");
            var htmlNodes = tRowNodes as HtmlNode[] ?? tRowNodes.ToArray();
            rowCount = htmlNodes.Count();

            if (tRowNodes == null || rowCount <= 0) return rowCount;

            for (var ctr = 1; ctr < htmlNodes.Count(); ctr++)
            {
                var protocolOk = true;
                var anonymityLevelOk = true;
                var speedOk = true;
                var connectionTimeOk = true;

                FreeProxy freeProxy = null;
                try
                {
                    if (ParseRowToProxy(htmlNodes.ToArray()[ctr], ref freeProxy))
                    {
                        if (freeProxy != null)
                        {
                            if (_protocolOptions != null && _protocolOptions.Count() > 0)
                            {
                                protocolOk = _protocolOptions.Any(x => x != freeProxy.Protocol);
                            }
                            else if (Protocol.HasValue)
                                protocolOk = (Protocol.Value == freeProxy.Protocol);

                            if (AnonymityLevel.HasValue)
                            {
                                if (AnonymityLevel.Value == ProxyAnonymityLevelEnum.High__KA)
                                    anonymityLevelOk = freeProxy.AnonymityLevel == ProxyAnonymityLevelEnum.High__KA;
                                else if (AnonymityLevel.Value == ProxyAnonymityLevelEnum.PlanetLab)
                                    anonymityLevelOk = freeProxy.AnonymityLevel == ProxyAnonymityLevelEnum.PlanetLab;
                                else
                                {
                                    if (AnonymityLevel.Value == ProxyAnonymityLevelEnum.High)
                                        anonymityLevelOk = freeProxy.AnonymityLevel == ProxyAnonymityLevelEnum.High;
                                    else if (AnonymityLevel.Value == ProxyAnonymityLevelEnum.Medium)
                                        anonymityLevelOk = freeProxy.AnonymityLevel == ProxyAnonymityLevelEnum.High |
                                                            freeProxy.AnonymityLevel == ProxyAnonymityLevelEnum.Medium;
                                    else if (AnonymityLevel.Value == ProxyAnonymityLevelEnum.Low)
                                        anonymityLevelOk = freeProxy.AnonymityLevel == ProxyAnonymityLevelEnum.High |
                                                            freeProxy.AnonymityLevel == ProxyAnonymityLevelEnum.Medium |
                                                            freeProxy.AnonymityLevel == ProxyAnonymityLevelEnum.Low;
                                    else if (AnonymityLevel.Value == ProxyAnonymityLevelEnum.None)
                                        anonymityLevelOk = freeProxy.AnonymityLevel == ProxyAnonymityLevelEnum.High |
                                                            freeProxy.AnonymityLevel == ProxyAnonymityLevelEnum.Medium |
                                                            freeProxy.AnonymityLevel == ProxyAnonymityLevelEnum.Low |
                                                            freeProxy.AnonymityLevel == ProxyAnonymityLevelEnum.None;
                                }

                            }

                            if (Speed.HasValue)
                            {
                                if (Speed.Value == ProxySpeedEnum.Fast)
                                    speedOk = freeProxy.Speed == ProxySpeedEnum.Fast;
                                else if (Speed.Value == ProxySpeedEnum.Medium)
                                    speedOk = freeProxy.Speed == ProxySpeedEnum.Medium | freeProxy.Speed == ProxySpeedEnum.Fast;
                                else
                                    speedOk = true;
                            }

                            if (ConnectionTime.HasValue)
                            {
                                if (ConnectionTime.Value == ProxyConnectionSpeedEnum.Fast)
                                    connectionTimeOk = freeProxy.ConnectionTime == ProxyConnectionSpeedEnum.Fast;
                                else if (ConnectionTime.Value == ProxyConnectionSpeedEnum.Medium)
                                    connectionTimeOk = freeProxy.ConnectionTime == ProxyConnectionSpeedEnum.Medium | freeProxy.ConnectionTime == ProxyConnectionSpeedEnum.Fast;
                                else
                                    connectionTimeOk = true;
                            }

                        }
                    }

                    if (protocolOk && anonymityLevelOk && speedOk && connectionTimeOk)
                    {
                        var exFreeProxy = FreeProxies
                            .ToArray().FirstOrDefault(p => p.HostIP == freeProxy.HostIP && p.PortNo == freeProxy.PortNo);

                        if (exFreeProxy == null)
                        {
                            lock (FreeProxies)
                            {
                                FreeProxies.Add(freeProxy);
                            }
                            InvokeEventFreeIPProxyFetched(new EventHandlers.FreeIPProxyFetchedEventArgs(freeProxy));
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return rowCount;
        }

        #region Event handlers

        public event EventHandlers.FreeIPProxyFetchedEventHandler OnFreeIPProxyFetched;
        public event EventHandler OnBeginningOfProxyListPage = delegate { };
        public event EventHandler OnEndOfProxyListPage = delegate { };
        public event EventHandlers.FreeIPGeneratorExceptionEventHandler OnFreeIPGeneratorException;

        public virtual void InvokeEventFreeIPProxyFetched(EventHandlers.FreeIPProxyFetchedEventArgs a)
        {
            var handler = OnFreeIPProxyFetched;
            if (handler != null)
                handler(this, a);
        }

        public virtual void InvokeEventFreeIPGeneratorException(EventHandlers.FreeIPGeneratorExceptionEventArgs a)
        {
            var handler = OnFreeIPGeneratorException;
            if (handler != null)
                handler(this, a);
        }

        #endregion
    }

}
