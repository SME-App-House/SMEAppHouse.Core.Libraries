using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using GPS.Frameworks.HtmlHelper;

namespace SMEAppHouse.Core.FreeProxyProvider
{
    public class FreeIPGenerator
    {
        private readonly object _locker = new object();
        public bool IsActive { get; private set; }

        private volatile bool m_resume = false;
        private volatile bool m_reset = false;

        public ProxyProtocolEnum? Protocol { get; private set; }
        public ProxyAnonymityLevelEnum? AnonymityLevel { get; private set; }
        public ProxySpeedEnum? Speed { get; private set; }
        public ProxyConnectionSpeedEnum? ConnectionTime { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public FreeIPGenerator()
            : this(null, null, null, null)
        {
        }
        public FreeIPGenerator(ProxyProtocolEnum? Protocol,
                                ProxyAnonymityLevelEnum? AnonymityLevel,
                                ProxySpeedEnum? Speed,
                                ProxyConnectionSpeedEnum? ConnectionTime)
        {

            if(Protocol.HasValue) this.Protocol = Protocol.Value;
            if(AnonymityLevel.HasValue) this.AnonymityLevel = AnonymityLevel.Value;
            if(Speed.HasValue) this.Speed = Speed.Value;
            if(ConnectionTime.HasValue) this.ConnectionTime = ConnectionTime.Value;

            new Task(() =>
            {
                this.RunIPProviderThread();
            })
            .Start();

        }

        /// <summary>
        /// FreeIPGeneratorException
        /// </summary>
        private void RunIPProviderThread()
        {
            lock(_locker)
            {
                string _hmaProxListUrl = "http://hidemyass.com/proxy-list/{0}";
                int _hmaPgIdx = 1;
                bool _pgEnding = false;

                do
                {
                    if(m_reset)
                    {
                        _hmaPgIdx = 1;
                        _pgEnding = false;
                        m_reset = false;
                    }

                    while(!_pgEnding)
                    {
                        if(m_resume)
                        {
                            if(_hmaPgIdx == 1 && OnBeginningOfProxyListPage != null)
                                OnBeginningOfProxyListPage(this, new EventArgs());

                            string __hmaProxListUrl = string.Format(_hmaProxListUrl, _hmaPgIdx);
                            string _hmaPgDoc = string.Empty;

                            try
                            {
                                _hmaPgDoc = HtmlUtil.GetPageDocument(__hmaProxListUrl, UserAgents.GetFakeUserAgent(UserAgents.UserAgent.Chrome));
                                _hmaPgDoc = _hmaPgDoc.Replace(System.Environment.NewLine, "");

                                int _start = _hmaPgDoc.IndexOf("<table id=\"listtable\"");
                                int _end = _hmaPgDoc.IndexOf("<div id=\"pagination\">");
                                _hmaPgDoc = _hmaPgDoc.Substring(_start, _end - _start);

                                int _results = this.ExtractProxies(_hmaPgDoc);

                                _hmaPgIdx++;

                                if(_results == 1)
                                {
                                    _hmaPgIdx = _hmaPgIdx--; // go back one page in index
                                    _pgEnding = true;

                                    if(OnEndOfProxyListPage != null)
                                        OnEndOfProxyListPage(this, new EventArgs());

                                }

                            }
                            catch(Exception ex)
                            {
                                string _errMsgPrefx = string.Format("Error@{0}/pgIdx:[{1}]/errmsg:[{2}]", "RunIPProviderThread", _hmaPgIdx, ex.Message);
                                Exception _ex = new Exception(_errMsgPrefx, ex);
                                this.InvokeEventFreeIPGeneratorException(new EventHandlers.FreeIPGeneratorExceptionEventArgs(_ex));
                            }
                        }
                        Thread.Sleep(1000 * 10);
                    }
                }
                while(true);
            }
        }

        public void Reset()
        {
            m_reset = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Resume()
        {
            m_resume = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            m_resume = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlString"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeHtml"></param>
        /// <returns></returns>
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
        private bool ParseRowToProxy( HtmlNode rowNode, ref FreeProxy freeProxy)
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
                        freeProxy.ConnectionTime = ProxyConnectionSpeedEnum.Slow;
                    else if(freeProxy.ConnectionTimeRate > 35 && freeProxy.ConnectionTimeRate <= 65)
                        freeProxy.ConnectionTime = ProxyConnectionSpeedEnum.Medium;
                    else if(freeProxy.ConnectionTimeRate > 65)
                        freeProxy.ConnectionTime = ProxyConnectionSpeedEnum.Medium;

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
        private int ExtractProxies(string hmaPgDoc)
        {
            int _rowCount = 0;

            HtmlAgilityPack.HtmlDocument _doc = new HtmlAgilityPack.HtmlDocument();
            _doc.LoadHtml(hmaPgDoc);

            var _tblNode = HtmlUtil.GetNode(_doc.DocumentNode, "table", "id", "listtable");
            if(_tblNode != null)
            {
                var _tRowNodes = _tblNode.Descendants("tr");
                _rowCount = _tRowNodes.Count();

                if(_tRowNodes != null && _rowCount > 0)
                {

                    Guid _usageId = Guid.NewGuid();

                    for(int ctr = 1; ctr < _tRowNodes.Count(); ctr++)
                    {
                        bool _ProtocolOK = true;
                        bool _AnonymityLevelOK = true;
                        bool _SpeedOK = true;
                        bool _ConnectionTimeOK = true;

                        FreeProxy _freeProxy = null;
                        try
                        {
                            if(ParseRowToProxy(_tRowNodes.ToArray()[ctr], ref _freeProxy))
                            {
                                if(_freeProxy != null)
                                {
                                    _freeProxy.RotationTokenId = _usageId;

                                    if(this.Protocol.HasValue)
                                        _ProtocolOK = (this.Protocol.Value == _freeProxy.Protocol);

                                    if(this.AnonymityLevel.HasValue)
                                        _AnonymityLevelOK = this.AnonymityLevel.Value == _freeProxy.AnonymityLevel;

                                    if(this.Speed.HasValue)
                                    {
                                        if(this.Speed.Value == ProxySpeedEnum.Fast)
                                            _SpeedOK = _freeProxy.Speed == ProxySpeedEnum.Fast;
                                        else if(this.Speed.Value == ProxySpeedEnum.Medium)
                                            _SpeedOK = _freeProxy.Speed == ProxySpeedEnum.Medium | _freeProxy.Speed == ProxySpeedEnum.Fast;
                                        else
                                            _SpeedOK = true;
                                    }

                                    if(this.ConnectionTime.HasValue)
                                    {
                                        if(this.ConnectionTime.Value == ProxyConnectionSpeedEnum.Fast)
                                            _ConnectionTimeOK = _freeProxy.ConnectionTime == ProxyConnectionSpeedEnum.Fast;
                                        else if(this.ConnectionTime.Value == ProxyConnectionSpeedEnum.Medium)
                                            _ConnectionTimeOK = _freeProxy.ConnectionTime == ProxyConnectionSpeedEnum.Medium | _freeProxy.ConnectionTime == ProxyConnectionSpeedEnum.Fast;
                                        else
                                            _ConnectionTimeOK = true;
                                    }

                                }
                            }

                            if(_ProtocolOK && _AnonymityLevelOK && _SpeedOK && _ConnectionTimeOK)
                            {
                                this.InvokeEventFreeIPProxyFetched(new EventHandlers.FreeIPProxyFetchedEventArgs(_freeProxy));
                            }
                        }
                        catch(Exception ex)
                        {
                            //Requested value 'LEBANON' was not found.
                        }
                    }
                }
            }
            return _rowCount;
        }

        #region Event handlers

        public event EventHandlers.FreeIPProxyFetchedEventHandler OnFreeIPProxyFetched;
        public event EventHandler OnBeginningOfProxyListPage = delegate { };
        public event EventHandler OnEndOfProxyListPage = delegate { };
        public event EventHandlers.FreeIPGeneratorExceptionEventHandler OnFreeIPGeneratorException;

        public virtual void InvokeEventFreeIPProxyFetched(EventHandlers.FreeIPProxyFetchedEventArgs a)
        {
            EventHandlers.FreeIPProxyFetchedEventHandler handler = OnFreeIPProxyFetched;
            if(handler != null)
                handler(this, a);
        }

        public virtual void InvokeEventFreeIPGeneratorException(EventHandlers.FreeIPGeneratorExceptionEventArgs a)
        {
            EventHandlers.FreeIPGeneratorExceptionEventHandler handler = OnFreeIPGeneratorException;
            if(handler != null)
                handler(this, a);
        }

        #endregion

    }
}
