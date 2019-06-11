using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Text.RegularExpressions;
using GPS.Frameworks.HtmlHelper;
using System.Threading.Tasks;

namespace GPS.Frameworks.FreeProxy
{
    public class FreeIPProvider
    {
        private readonly object _locker = new object();

        public Guid UsageTokenId { get; private set; }
        public Queue<FreeProxy> FreeProxies { get; set; }
        public bool IsActive { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public FreeIPProvider()
        {
            UsageTokenId = Guid.NewGuid();

            new Task(() =>
            {
                this.RunIPProviderThread();
            })
            .Start();

            //while(this.FreeProxies == null || this.FreeProxies.Count == 0)
            //{
            //    Thread.Sleep(10);
            //}

        }

        /// <summary>
        /// 
        /// </summary>
        private void RunIPProviderThread()
        {
            lock(_locker)
            {
                string _hmaProxListUrl = "http://hidemyass.com/proxy-list/{0}";
                int _hmaPgIdx = 0;
                bool _pgNotEnding = false;

                while(true)
                {
                    while(!_pgNotEnding)
                    {
                        if(FreeProxies == null)
                            FreeProxies = new Queue<FreeProxy>();

                        _hmaPgIdx++;
                        string __hmaProxListUrl = string.Format(_hmaProxListUrl, _hmaPgIdx);
                        string _hmaPgDoc = string.Empty;

                        try
                        {
                            _hmaPgDoc = HtmlUtil.GetPageDocument(__hmaProxListUrl, UserAgents.GetFakeUserAgent(UserAgents.UserAgent.Chrome));
                            _hmaPgDoc = _hmaPgDoc.Replace(System.Environment.NewLine, "");

                            int _start = _hmaPgDoc.IndexOf("<table id=\"listtable\"");
                            int _end = _hmaPgDoc.IndexOf("<div id=\"pagination\">");
                            _hmaPgDoc = _hmaPgDoc.Substring(_start, _end - _start);

                            this.ExtractProxies(_hmaPgDoc);
                        }
                        catch(Exception ex)
                        {
                            _pgNotEnding = true;
                        }
                    }
                    Thread.Sleep(1000 * 60 * 10);
                }
            }
        }

        private List<string> CollectUnsanitizadTags(string htmlString)
        {
            List<string> _unsanitizadTags = null;
            string pattern = "<style[^<]*</style>";
            Regex rgx = new Regex(pattern);

            foreach(Match match in rgx.Matches(htmlString))
            {
                if(_unsanitizadTags == null) _unsanitizadTags = new List<string>();
                _unsanitizadTags.Add(match.Value);
            }
            return _unsanitizadTags;
        }

        private string GetStyleIDString(string nodeHtml)
        {
            string _styleStr = string.Empty;
            try
            {
                string pattern = "<style[^<]*</style>";
                Regex rgx = new Regex(pattern);
                var v = rgx.Match(nodeHtml);
                _styleStr = v.Groups[0].ToString();
                _styleStr = _styleStr.Replace(" ", "").Trim();
            }
            catch(Exception ex)
            { }
            return _styleStr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="styleIds"></param>
        /// <param name="toOmit"></param>
        /// <param name="toOmitNot"></param>
        private void AnalyzeHidingStyles(string styleIds, ref string[] toOmit, ref string[] toNotOmit)
        {
            List<string> _toOmit = new List<string>();
            List<string> _toNotOmit = new List<string>();

            string[] _styleItms = styleIds
                                    .Replace("<style>", "")
                                    .Replace("</style>", "")
                                    .Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            if(_styleItms != null && _styleItms.Length > 0)
            {
                foreach(string s in _styleItms)
                {
                    if(s.Contains("display:none"))
                        _toOmit.Add(s.Replace(".", "").Replace("{display:none}", ""));
                    else if(s.Contains("display:inline"))
                        _toNotOmit.Add(s.Replace(".", "").Replace("{display:inline}", ""));
                }
                toOmit = _toOmit.ToArray();
                toNotOmit = _toNotOmit.ToArray();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeHtml"></param>
        /// <returns></returns>
        private string SnatchIPAddress(string nodeHtml)
        {
            string _styleIds = GetStyleIDString(nodeHtml);
            string[] _toOmit = null;
            string[] _toNotOmit = null;

            AnalyzeHidingStyles(_styleIds, ref _toOmit, ref _toNotOmit);

            HtmlAgilityPack.HtmlDocument _doc = new HtmlAgilityPack.HtmlDocument();
            _doc.LoadHtml(nodeHtml.Replace(_styleIds, "").Replace("<span></span>", ""));

            var _nodes = _doc.DocumentNode.Descendants().ToArray();
            string _final = _doc.DocumentNode.InnerHtml;

            for(int ctr = 1; ctr < _nodes.Count(); ctr++)
            {
                try
                {
                    var _node = _nodes[ctr];
                    string _test = _node.OuterHtml;

                    if(!_node.Name.Equals("#text"))
                    {
                        string _replaceStr = string.Empty;
                        string _className = string.Empty;
                        string _style = string.Empty;

                        if(_node.Attributes["style"] != null &&
                            _node.Attributes["style"].Value.Replace(" ", "").Equals("display:none"))
                            _replaceStr = "";
                        else if(_node.Attributes["style"] != null &&
                            _node.Attributes["style"].Value.Replace(" ", "").Equals("display:inline"))
                            _replaceStr = _node.InnerText;

                        else if(_node.Attributes["class"] != null)
                        {
                            _className = _node.Attributes["class"].Value;

                            if(_toOmit.Contains(_className))
                                _replaceStr = string.Empty;
                            else if(_toNotOmit.Contains(_className))
                                _replaceStr = _node.InnerText;
                            else
                                _replaceStr = _node.InnerText;
                        }
                        _final = _final.Replace(_node.OuterHtml, _replaceStr);
                    }

                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
            return _final.Replace("<span>", "").Replace("</span>", "");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rowNode"></param>
        /// <param name="freeProxy"></param>
        /// <returns></returns>
        private bool ParseRowToProxy(HtmlAgilityPack.HtmlNode rowNode, ref FreeProxy freeProxy)
        {
            try
            {
                var _prxCells = rowNode.Descendants("td");

                if(_prxCells != null && _prxCells.Count() > 0)
                {
                    freeProxy = new FreeProxy();
                    freeProxy.Host = SnatchIPAddress(_prxCells.ToArray()[1].InnerHtml);
                    freeProxy.Port = int.Parse(_prxCells.ToArray()[2].InnerText);

                    string _country = _prxCells.ToArray()[3].InnerText.Trim().ToUpper();
                    _country = _country.Replace("; ", "___").Replace(", ", "__").Replace(" ", "_");
                    freeProxy.Country = (ProxyCountry)Enum.Parse(typeof(ProxyCountry), _country);

                    var _connspeedNode = _prxCells.ToArray()[5];
                    _connspeedNode = HtmlUtil.GetNode(_connspeedNode, "div", "class", "speedbar connection_time");
                    _connspeedNode = _connspeedNode.Descendants("div").ToArray()[0];
                    string _rate = _connspeedNode.Attributes["style"].Value.Replace("width:", "").Replace("%", "");
                    freeProxy.ConnectionTimeRate = int.Parse(_rate);

                    if(freeProxy.ConnectionTimeRate <= 35)
                        freeProxy.ConnectionTime = ProxyConnectionTimeEnum.Slow;
                    else if(freeProxy.ConnectionTimeRate > 35 && freeProxy.ConnectionTimeRate <= 65)
                        freeProxy.ConnectionTime = ProxyConnectionTimeEnum.Medium;
                    else if(freeProxy.ConnectionTimeRate > 65)
                        freeProxy.ConnectionTime = ProxyConnectionTimeEnum.Medium;

                    var _speedNode = _prxCells.ToArray()[4];
                    _speedNode = HtmlUtil.GetNode(_speedNode, "div", "class", "speedbar response_time");
                    _speedNode = _speedNode.Descendants("div").ToArray()[0];
                    _rate = _speedNode.Attributes["style"].Value.Replace("width:", "").Replace("%", "");
                    freeProxy.SpeedRate = int.Parse(_rate);

                    if(freeProxy.SpeedRate <= 35)
                        freeProxy.Speed = ProxySpeedEnum.Slow;
                    else if(freeProxy.SpeedRate > 35 && freeProxy.SpeedRate <= 65)
                        freeProxy.Speed = ProxySpeedEnum.Medium;
                    else if(freeProxy.SpeedRate > 65)
                        freeProxy.Speed = ProxySpeedEnum.Medium;


                    string _protocol = _prxCells.ToArray()[6].InnerText.Replace("/", "_");
                    freeProxy.Protocol = (ProxyProtocolEnum)Enum.Parse(typeof(ProxyProtocolEnum), _protocol);

                    string _anonymity = _prxCells.ToArray()[7].InnerText.Replace(" +", "__");
                    freeProxy.AnonymityLevel = (ProxyAnonymityLevelEnum)Enum.Parse(typeof(ProxyAnonymityLevelEnum), _anonymity);
                    return true;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hmaPgDoc"></param>
        private void ExtractProxies(string hmaPgDoc)
        {
            HtmlAgilityPack.HtmlDocument _doc = new HtmlAgilityPack.HtmlDocument();
            _doc.LoadHtml(hmaPgDoc);

            var _tblNode = HtmlUtil.GetNode(_doc.DocumentNode, "table", "id", "listtable");
            if(_tblNode != null)
            {
                var _tRowNodes = _tblNode.Descendants("tr");
                if(_tRowNodes != null && _tRowNodes.Count() > 0)
                {
                    if(this.FreeProxies == null)
                        this.FreeProxies = new Queue<FreeProxy>();

                    for(int ctr = 1; ctr < _tRowNodes.Count(); ctr++)
                    {
                        FreeProxy _freeProxy = null;
                        try
                        {
                            if(ParseRowToProxy(_tRowNodes.ToArray()[ctr], ref _freeProxy))
                            {
                                if(_freeProxy != null && !this.FreeProxies.ToList().Exists(p => p.Host.Equals(_freeProxy.Host)))
                                {
                                    this.FreeProxies.Enqueue(_freeProxy);
                                }
                            }
                        }
                        catch(Exception ex)
                        {
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public FreeProxy GetFreeProxy()
        {
            return GetFreeProxy(ProxyProtocolEnum.HTTP, ProxyAnonymityLevelEnum.High, 35, 35);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Protocol"></param>
        /// <param name="AnonymityLevel"></param>
        /// <param name="SpeedRateAtLeast"></param>
        /// <param name="ConnectionTimeRateAtLeast"></param>
        /// <returns></returns>
        public FreeProxy GetFreeProxy(ProxyProtocolEnum Protocol = ProxyProtocolEnum.HTTP
                                        , ProxyAnonymityLevelEnum AnonymityLevel = ProxyAnonymityLevelEnum.High
                                        , int SpeedRateAtLeast = 35
                                        , int ConnectionTimeRateAtLeast = 35)
        {
            while(this.FreeProxies == null || this.FreeProxies.Count == 0)
            {
                Thread.Sleep(10);
            }

        _again:

            int _faultCount = 0;
            FreeProxy _freePrx = null;

            try
            {

                var _proxies = FreeProxies.ToArray().Where(p => p.UsageTokenId != UsageTokenId &&
                                                        p.Protocol == Protocol &&
                                                        p.AnonymityLevel == AnonymityLevel &&
                                                        p.SpeedRate >= SpeedRateAtLeast &&
                                                        p.ConnectionTimeRate >= ConnectionTimeRateAtLeast);

                if(_proxies == null || _proxies.Count() == 0)
                    UsageTokenId = Guid.NewGuid();

                _freePrx = FreeProxies.ToArray().Where(p => p.UsageTokenId != UsageTokenId &&
                                                            p.Protocol == Protocol &&
                                                            p.AnonymityLevel == AnonymityLevel &&
                                                            p.SpeedRate >= SpeedRateAtLeast &&
                                                            p.ConnectionTimeRate >= ConnectionTimeRateAtLeast).FirstOrDefault();
            }
            catch(ArgumentException ex)
            {
                if(_faultCount > 3)
                {
                    throw ex;
                }
                else
                {
                    _faultCount++;
                    Thread.Sleep(1000);
                    goto _again;
                }
            }

            if(_freePrx != null)
                _freePrx.UsageTokenId = UsageTokenId;

            return _freePrx;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Protocol"></param>
        /// <param name="AnonymityLevel"></param>
        /// <param name="Speed"></param>
        /// <param name="ConnectionTime"></param>
        /// <returns></returns>
        public FreeProxy GetFreeProxy(ProxyProtocolEnum Protocol = ProxyProtocolEnum.HTTP
                                        , ProxyAnonymityLevelEnum AnonymityLevel = ProxyAnonymityLevelEnum.High
                                        , ProxySpeedEnum Speed = ProxySpeedEnum.Fast
                                        , ProxyConnectionTimeEnum ConnectionTime = ProxyConnectionTimeEnum.Fast)
        {
            while(this.FreeProxies == null || this.FreeProxies.Count == 0)
            {
                Thread.Sleep(10);
            }

        _again:

            int _faultCount = 0;
            FreeProxy _freePrx = null;

            try
            {

                var _proxies = FreeProxies.ToArray().Where(p => p.UsageTokenId != UsageTokenId &&
                                                        p.Protocol == Protocol &&
                                                        p.AnonymityLevel == AnonymityLevel &&
                                                        p.Speed == Speed &&
                                                        p.ConnectionTime == ConnectionTime);

                if(_proxies == null || _proxies.Count() == 0)
                    UsageTokenId = Guid.NewGuid();

                _freePrx = FreeProxies.ToArray().Where(p => p.UsageTokenId != UsageTokenId &&
                                                            p.Protocol == Protocol &&
                                                            p.AnonymityLevel == AnonymityLevel &&
                                                            p.Speed == Speed &&
                                                            p.ConnectionTime == ConnectionTime).FirstOrDefault();
            }
            catch(ArgumentException ex)
            {
                if(_faultCount > 3)
                {
                    throw ex;
                }
                else
                {
                    _faultCount++;
                    Thread.Sleep(1000);
                    goto _again;
                }
            }
            if(_freePrx != null)
                _freePrx.UsageTokenId = UsageTokenId;
            return _freePrx;
        }

    }

    public class FreeProxy
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public int ConnectionTimeRate { get; set; }
        public int SpeedRate { get; set; }

        public ProxyCountry Country { get; set; }
        public ProxyProtocolEnum Protocol { get; set; }
        public ProxyAnonymityLevelEnum AnonymityLevel { get; set; }
        public ProxySpeedEnum Speed { get; set; }
        public ProxyConnectionTimeEnum ConnectionTime { get; set; }

        public Guid UsageTokenId { get; set; }

    }

    public enum ProxyProtocolEnum
    {
        HTTP, HTTPS, socks4_5
    }

    public enum ProxyAnonymityLevelEnum
    {
        //PlanetLab may require Captcha
        None, Low, Medium, High, High__KA, PlanetLab
    }

    public enum ProxySpeedEnum
    {
        Slow, Medium, Fast
    }

    public enum ProxyConnectionTimeEnum
    {
        Slow, Medium, Fast
    }

    /// <summary>
    /// 
    /// ' ':'_'
    /// ', '-'__'
    /// ", ", "__"
    /// </summary>
    public enum ProxyCountry
    {
        CHINA,
        INDONESIA,
        UNITED_STATES,
        BRAZIL,
        VENEZUELA,
        KAZAKHSTAN,
        RUSSIAN_FEDERATION,
        IRAN,
        UKRAINE,
        EGYPT,
        POLAND,
        INDIA,
        GERMANY,
        THAILAND,
        COLOMBIA,
        BANGLADESH,
        NETHERLANDS,
        CHILE,
        ECUADOR,
        UNITED_ARAB_EMIRATES,
        BULGARIA,
        SERBIA,
        TAIWAN,
        LATVIA,
        FRANCE,
        CZECH_REPUBLIC,
        HONG_KONG,
        MOLDOVA__REPUBLIC_OF,
        CAMBODIA,
        KOREA__REPUBLIC_OF,
        BOSNIA_AND_HERZEGOVINA,
        IRAQ,
        ROMANIA,
        PHILIPPINES,
        PERU,
        NIGERIA,
        SLOVENIA,
        TURKEY,
        KENYA,
        PAKISTAN,
        MEXICO,
        ARGENTINA,
        ITALY,
        MACEDONIA,
        MALAYSIA,
        HONDURAS,
        CANADA,
        AUSTRALIA,
        SPAIN,
        VIET_NAM,
        MONGOLIA,
        HUNGARY,
        PALESTINIAN_TERRITORY__OCCUPIED,
        ALBANIA,
        MADAGASCAR,
        SLOVAKIA,
        JAPAN,
        UZBEKISTAN,
        SINGAPORE,
        NAMIBIA,
        LIBYAN_ARAB_JAMAHIRIYA,
        SWITZERLAND,
        JORDAN,
        SAUDI_ARABIA,
        PANAMA,
        EUROPE,
        NETHERLANDS_ANTILLES,
        ICELAND,
        COSTA_RICA,
        DENMARK,
        ZIMBABWE,
        BOLIVIA,
        SWEDEN,
        GUATEMALA,
        DOMINICAN_REPUBLIC,
        ESTONIA,
        ARMENIA,
        AZERBAIJAN,
        BELARUS,
        FINLAND,
        TURKMENISTAN,
        TANZANIA__UNITED_REPUBLIC_OF,
        NEW_ZEALAND,
        ZAMBIA,
        AUSTRIA,
        GHANA,
        UNITED_KINGDOM,
        CROATIA,
        GREECE,
    }
}
