using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using HtmlAgilityPack;
using ScrapySharp.Network;
using SMEAppHouse.Core.CodeKits;
using SMEAppHouse.Core.CodeKits.Extensions;
using SMEAppHouse.Core.ScraperBox.Models;
using static SMEAppHouse.Core.ScraperBox.Models.PageInstruction;

namespace SMEAppHouse.Core.ScraperBox
{
    public static class Helper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="countryNamePartial"></param>
        /// <returns></returns>
        public static Rules.WorldCountriesEnum FindProxyCountryFromPartial(string countryNamePartial)
        {
            var proxyCountries = EnumExt.GetEnumNames<Rules.WorldCountriesEnum>();
            var target = string.Empty;

            foreach (var x in proxyCountries.Where(x => x.ToLower().Contains(countryNamePartial.ToLower().Trim())))
            {
                target = x;
                break;
            }

            if (string.IsNullOrEmpty(target))
                return Rules.WorldCountriesEnum.UNKNOWN;

            var pxycountry = (Rules.WorldCountriesEnum) Enum.Parse(typeof(Rules.WorldCountriesEnum), target, true);
            return pxycountry;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pgInstruction"></param>
        /// <param name="pgNo"></param>
        /// <returns></returns>
        public static string PageNo(this PageInstruction pgInstruction, int pgNo)
        {
            if (pgNo.ToString().Length > pgInstruction.PadLength)
                throw new InvalidOperationException(
                    "Page number character length exceeds page instruction pad length.");
            var _pgNo = pgNo.ToString();
            if (pgInstruction.PaddingDirection == PaddingDirectionsEnum.ToLeft)
                return _pgNo.PadLeft(pgInstruction.PadLength, pgInstruction.PadCharacter);
            else return _pgNo.PadRight(pgInstruction.PadLength, pgInstruction.PadCharacter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string ResolveHttpUrl(string url)
        {
            return url.Substring(0, 2).Contains("//")
                ? $"http:{url}"
                : url;
        }

        ///// <summary>
        ///// http://stackoverflow.com/questions/9793160/getting-the-innerhtml-of-an-htmltable-c-sharp
        ///// </summary>
        ///// <param name="htmlTable"></param>
        ///// <returns></returns>
        //public static string HtmlTableToHtmlString(HtmlTable htmlTable)
        //{
        //    var sb = new StringBuilder();
        //    var tw = new StringWriter(sb);
        //    var hw = new HtmlTextWriter(tw);

        //    htmlTable.RenderControl(hw);
        //    var htmlContent = sb.ToString();
        //    return htmlContent;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        /// <param name="allTrim"></param>
        /// <param name="otherElementsToClear"></param>
        /// <returns></returns>
        public static string Resolve(string val, bool allTrim = false, params string[] otherElementsToClear)
        {
            val = val.Replace("&amp;", "&");
            val = val.TrimEnd(',');
            val = val.Replace("%3A", ":").Replace("%2F", "/");
            val = val.Replace("&#034;", "\"");
            val = val.Replace("&#039;", "'");
            val = val.Replace("<br />", "\n");
            val = val.Replace("<br/>", "\n");
            val = val.Replace("&nbsp;", " ");

            if (otherElementsToClear.Any())
            {
                otherElementsToClear.ToList().ForEach(e => { val = val.Replace(e, ""); });
            }

            if (allTrim) val = val.Trim();
            return val;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        /// <param name="allTrim"></param>
        /// <returns></returns>
        public static string CleanupHtmlStrains(string val, bool allTrim = false)
        {
            val = val.Replace("&amp;", "");
            val = val.TrimEnd(',');
            val = val.Replace("%3A", "");
            val = val.Replace("%2F", "");
            val = val.Replace("&#034;", "");
            val = val.Replace("&#039;", "");
            val = val.Replace("<br />", "");
            val = val.Replace("<br/>", "");
            val = val.Replace("&nbsp;", "");
            val = val.Replace("amp;", "");
            val = val.Replace("#shId", "");
            val = val.Replace(System.Environment.NewLine, "");
            val = val.Replace("\n", "");

            if (allTrim) val = val.Trim();
            return val;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string EncodeQueryStringSegment(string query)
        {
            return query.ToLower()
                .Trim()
                .Replace("&", "%26")
                .Replace(" ", "%20");
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="retainHttPrefix"></param>
        /// <returns></returns>
        public static string ExtractDomainNameFromUrl(string url, bool retainHttPrefix = false)
        {
            string httPrefix = string.Empty;

            if (url.Contains(@"://"))
            {
                httPrefix = url.Split(new string[] {"://"}, StringSplitOptions.None)[0];
                url = url.Split(new string[] {"://"}, 2, StringSplitOptions.None)[1];
            }

            if (retainHttPrefix)
                return $"{httPrefix}://{url.Split('/')[0]}";
            else
                return url.Split('/')[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetPageDocumentWithCookie(string url)
        {
            var myCookieColl = new CookieCollection();
            var pageContent = string.Empty;

            try
            {

                /*
                     gso-cookie=<HBD><LANGUAGE>0</LANGUAGE><CREATE_DATE>2013-03-22 10:23:50</CREATE_DATE><LAST_ACCESS>2013-03-23 03:22:28</LAST_ACCESS><CLIENT_ID>13639442303730103</CLIENT_ID></HBD>; 
                    __CT_Data=gpv=21&apv_5550_www09=6&apv_5553_www09=15; 
                    WRUID=0; 
                    __atuvc=17|12; 
                    POPUPCHECK=1364090480993; 
                    rsi_segs=K12389_10001|K12389_0; 
                    s_cc=true; 
                    s_sq=edsaherold.at=&pid=result&pidt=1&oid=javascript:document.getElementById('f_smallform').submit();&ot=A; 
                    s_ppv=100; 
                    hbd-scn=<HBD><SESSION_ID>PWA6_13640050053120106</SESSION_ID></HBD>; 
                    edsa_stc=rebschulen
                 */

                //string _gso_cookie = "<HBD><LANGUAGE>0</LANGUAGE><CREATE_DATE>2013-03-22 10:23:50</CREATE_DATE><LAST_ACCESS>2013-03-23 03:22:28</LAST_ACCESS><CLIENT_ID>13639442303730103</CLIENT_ID></HBD>";
                //string ___CT_Data = "gpv=21&apv_5550_www09=6&apv_5553_www09=15";
                //string _WRUID = "0";
                //string __atuvc = "17|12";
                //string _POPUPCHECK = "1364090480993";
                //string _rsi_segs = "K12389_10001|K12389_0";
                //string _s_cc = "true";
                //string _s_sq = "edsaherold.at=&pid=result&pidt=1&oid=javascript:document.getElementById('f_smallform').submit();&ot=A";
                //string _s_ppv = "100";
                //string _hbd_scn = "<HBD><SESSION_ID>PWA6_13640050053120106</SESSION_ID></HBD>";
                //string _edsa_stc = "rebschulen";

                var cookie = new StringBuilder();
                cookie.Append("plk=G; ");
                cookie.Append("PJSESSIONID=858D856B61E4BC61203A258DE7D65D90.yas61g; ");
                cookie.Append("cookieILOSEO=858D856B61E4BC61203A258DE7D65D90.yas61g; ");
                cookie.Append("VisitorID=089136419433373747; ");
                cookie.Append("IdPage=0891364194333736221364194333736; ");
                cookie.Append("IdPagePrecedent=0; ");
                cookie.Append("xtvrn=$483323$; ");
                cookie.Append("xtan=-; ");
                cookie.Append("xtant=1; ");
                cookie.Append("OAX=fdR5J1FP9CMADG8k; ");
                cookie.Append("rdmvalidation=0; ");
                cookie.Append("Date1stVisit=Mon Mar 25 2013 14:52:17 GMT 0800 (China Standard Time); ");
                cookie.Append("cookieEnabled=OK; ");
                cookie.Append("trackEnabled=0");

                var domain = ExtractDomainNameFromUrl(url, true);

                var myNameValueCollection = new NameValueCollection();
                myNameValueCollection.Add("quiQuoiSaisi", null);
                myNameValueCollection.Add("quiQuoiNbCar", null);
                myNameValueCollection.Add("ouSaisi", null);
                myNameValueCollection.Add("ouNbCar", null);
                myNameValueCollection.Add("quoiqui", "restaurant");
                myNameValueCollection.Add("ou", "paris");

                //first request
                var cookies = new CookieCollection();
                var request = (HttpWebRequest) WebRequest.Create(domain);
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);

                //Get the response from the server and save the cookies from the first request..
                var response = (HttpWebResponse) request.GetResponse();
                cookies = response.Cookies;


                var getUrl = url;

                var getRequest = (HttpWebRequest) WebRequest.Create(getUrl);
                getRequest.CookieContainer = new CookieContainer();
                getRequest.CookieContainer.Add(cookies); //recover cookies First request
                getRequest.Method = WebRequestMethods.Http.Post;
                getRequest.UserAgent =
                    "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
                getRequest.AllowWriteStreamBuffering = true;
                getRequest.ProtocolVersion = HttpVersion.Version11;
                getRequest.AllowAutoRedirect = true;
                getRequest.ContentType = "application/x-www-form-urlencoded";

                var byteArray = Encoding.ASCII.GetBytes(cookie.ToString());
                getRequest.ContentLength = byteArray.Length;

                var newStream = getRequest.GetRequestStream(); //open connection
                newStream.Write(byteArray, 0, byteArray.Length); // Send the data.

                newStream.Close();

                var getResponse = (HttpWebResponse) getRequest.GetResponse();
                using (var sr = new StreamReader(getResponse.GetResponseStream()))
                {
                    var sourceCode = sr.ReadToEnd();
                }

                WebClient myWebClient = new WebClient();
                byte[] responseArray = myWebClient.UploadValues(domain, myNameValueCollection);
                string _response = Encoding.ASCII.GetString(responseArray);

            }
            catch (Exception ex)
            {
                pageContent = $"[ERROR_@_{"GetPageDocumentWithCookie"}:{ex.Message}]";
            }

            return pageContent;

        }


        #region GetPageDocument

        public static string GetPageDocument(string site)
        {
            return GetPageDocument(site, null);
        }

        public static string GetPageDocument(string site,
            FakeUserAgent userAgent,
            bool ignoreCookies = true,
            bool useDefaultCookiesParser = false)
        {
            if (!site.Contains("http://")) site = $"http://{site}";
            return GetPageDocument(new Uri(site), null, userAgent, ignoreCookies, useDefaultCookiesParser);
        }

        public static string GetPageDocument(string site, ref string extraDataOnError)
        {
            return GetPageDocument(site, null, ref extraDataOnError);
        }

        public static string GetPageDocument(string site, IWebProxy webProxy, ref string extraDataOnError)
        {
            return GetPageDocument(new Uri(site), webProxy, null);
        }



        //System.Net.CookieException

        /// <summary>
        /// 
        /// </summary>
        /// <param name="site"></param>
        /// <param name="webProxy"></param>
        /// <param name="userAgent"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static string GetPageDocument(Uri site,
            IWebProxy webProxy = null,
            FakeUserAgent userAgent = null,
            params KeyValuePair<string, string>[] headers)
        {
            return GetPageDocument(site,
                webProxy,
                userAgent,
                true,
                false,
                headers);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceUrl"></param>
        /// <param name="extraDataOnError"></param>
        /// <param name="webProxy"></param>
        /// <param name="retries"></param>
        /// <returns></returns>
        public static string GetPageDocument(string sourceUrl, ref string extraDataOnError, IWebProxy webProxy = null,
            int retries = 2)
        {
            Exception exception = null;
            var failCnt = 0;
            do
            {
                try
                {
                    var uAgent =
                        "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/29.0.1547.2 Safari/537.36";
                    var htmlDoc = GetPageDocument(new Uri(sourceUrl), webProxy, new FakeUserAgent("Chrome", uAgent));
                    return htmlDoc;
                }
                catch (WebException ex)
                {
                    exception = ex;
                    throw exception;
                }
                catch (Exception ex)
                {
                    var w = (WebProxy) webProxy;
                    var probeMsg = $"failed to respond {w.Address.Host}:{w.Address.Port}";

                    if (ex.Message.Contains(probeMsg))
                    {
                        throw new WebException("proxy is invalid");
                    }
                    else
                    {
                        failCnt++;
                        ex = ex;

                        if (failCnt < retries)
                            Thread.Sleep(1000 * failCnt);
                    }
                }
            } while (failCnt != retries);

            if (failCnt == retries && exception != null)
                throw exception;

            return string.Empty;
        }

        public static string GetPageDocument(Uri site,
            IWebProxy webProxy = null,
            FakeUserAgent userAgent = null,
            bool? ignoreCookies = true,
            bool? useDefaultCookiesParser = false,
            params KeyValuePair<string, string>[] headers)
        {
            var redirectedUrl = string.Empty;
            return GetPageDocument(site, ref redirectedUrl,
                webProxy,
                userAgent,
                ignoreCookies,
                useDefaultCookiesParser,
                headers);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="site"></param>
        /// <param name="webProxy"></param>
        /// <param name="userAgent"></param>
        /// <param name="ignoreCookies"></param>
        /// <param name="useDefaultCookiesParser"></param>
        /// <param name="redirectedUrl"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static string GetPageDocument(Uri site,
            ref string redirectedUrl,
            IWebProxy webProxy = null,
            FakeUserAgent userAgent = null,
            bool? ignoreCookies = true,
            bool? useDefaultCookiesParser = false,
            params KeyValuePair<string, string>[] headers)
        {
            if (userAgent == null)
                userAgent = UserAgents.GetFakeUserAgent(UserAgents.Mozilla22);

            try
            {
                ScrapingBrowser browser = null;
                if (webProxy != null)
                {
                    browser = new ScrapingBrowser()
                    {
                        UserAgent = userAgent,
                        Proxy = webProxy,
                    };
                }
                else
                {
                    Dictionary<string, string> hdrs = null;

                    if (headers != null && headers.Any())
                    {
                        hdrs = new Dictionary<string, string>();
                        headers.ToList().ForEach(h => { hdrs.Add(h.Key, h.Value); });

                    }

                    browser = new ScrapingBrowser
                    {
                        UserAgent = userAgent,
                        //KeepAlive = true,
                    };


                }

                browser.IgnoreCookies = ignoreCookies.GetValueOrDefault();
                browser.AllowMetaRedirect = true;
                browser.Timeout = new TimeSpan(0, 0, 30);
                //useDefaultCookiesParser.GetValueOrDefault();

                var html = browser.DownloadString(site);

                redirectedUrl = browser.Referer.AbsoluteUri;
                return html;
            }
            catch (WebException webEx)
            {
                //var extraDataOnError = string.Empty;
                //if (webEx.Response != null)
                //{
                //    var stream = webEx.Response.GetResponseStream();
                //    if (stream != null)
                //        using (var sr = new StreamReader(stream))
                //            extraDataOnError = sr.ReadToEnd();
                //}

                // Now you can access webEx.Response object that contains more info on the server response              
                if (webEx.Status != WebExceptionStatus.ProtocolError)
                    throw;

                var response = ((HttpWebResponse) webEx.Response);

                if (response == null) return string.Empty;
                var error = $"Error occurred: {response.StatusCode}";

                if (webEx.InnerException == null)
                    error += $"; Description : {response.StatusDescription}";
                else
                    error += $"; Description : {response.StatusDescription}; {webEx.InnerException.Message}";

                throw new WebException(error, webEx.InnerException);
            }
            catch (AggregateException aEx)
            {
                var sErr = string.Empty;
                aEx.Handle((x) =>
                {
                    if (x is UnauthorizedAccessException) // This we know how to handle.
                    {
                        //do your code here  
                    }
                    else
                    {
                        sErr += x.Message;
                    }

                    return true; //if you do something like this all exceptions are marked as handled  
                });
                throw new Exception(sErr);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceHtml"></param>
        /// <returns></returns>
        public static string RemoveHtmlComments(string sourceHtml)
        {
            var output = string.Empty;
            var temp = System.Text.RegularExpressions.Regex.Split(sourceHtml, "<!--");
            return (from s in temp
                    let str = string.Empty
                    select !s.Contains("-->") ? s : s.Substring(s.IndexOf("-->", StringComparison.Ordinal) + 3)
                    into str
                    where str.Trim() != string.Empty
                    select str)
                .Aggregate(output, (current, str) => current + str.Trim());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string RemoveUnwantedTags(string data)
        {
            var acceptableUnwantedTags = new string[] {"strong", "em", "u"};
            return RemoveUnwantedTags(data, acceptableUnwantedTags);
        }


        /// <summary>
        /// http://stackoverflow.com/questions/12787449/html-agility-pack-removing-unwanted-tags-without-removing-content
        /// </summary>
        /// <param name="data"></param>
        /// <param name="acceptableTags"></param>
        /// <returns></returns>
        public static string RemoveUnwantedTags(string data, string[] acceptableTags)
        {
            var document = new HtmlDocument();
            document.LoadHtml(data);

            var nodes = new Queue<HtmlNode>(document.DocumentNode.SelectNodes("./*|./text()"));
            while (nodes.Count > 0)
            {
                var node = nodes.Dequeue();
                var parentNode = node.ParentNode;

                if (!acceptableTags.Contains(node.Name) && node.Name != "#text")
                {
                    var childNodes = node.SelectNodes("./*|./text()");

                    if (childNodes != null)
                    {
                        foreach (var child in childNodes)
                        {
                            nodes.Enqueue(child);
                            parentNode.InsertBefore(child, node);
                        }
                    }

                    parentNode.RemoveChild(node);

                }
            }

            return document.DocumentNode.InnerHtml;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceNode"></param>
        /// <param name="element"></param>
        /// <param name="attribute"></param>
        /// <param name="valuePartial"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetInnerText(HtmlNode sourceNode,
            string element,
            string attribute,
            string valuePartial,
            string defaultValue = "")
        {
            var node = sourceNode
                .Descendants(element)
                .FirstOrDefault(d =>
                    d.Attributes.Contains(attribute) && d.Attributes[attribute].Value.Contains(valuePartial));

            return node == null ? defaultValue : node.InnerText;
        }

        public static string GetInnerText(HtmlNode node, params string[] tagsToRemove)
        {
            return GetInnerText(node, false, tagsToRemove);
        }


        public static string GetInnerText(HtmlNode node, bool removeCommentTags = true, params string[] tagsToRemove)
        {
            node.Descendants()
                //.Where(n => n.Name == "script" || n.Name == "style")
                .ToList()
                .ForEach(n =>
                {
                    if (tagsToRemove.Contains(n.Name))
                        n.Remove();
                });

            if (!removeCommentTags) return node.InnerText;
            try
            {
                foreach (var comment in node.SelectNodes("//comment()"))
                {
                    comment.ParentNode.RemoveChild(comment);
                }
            }
            catch (Exception ex)
            {
            }

            return node.InnerText;
        }

        public static string GetInnerText(string sourceHtml, params string[] tagsToRemove)
        {
            return GetInnerText(sourceHtml, false, tagsToRemove);
        }

        public static string GetInnerText(string sourceHtml, bool removeCommentTags = true,
            params string[] tagsToRemove)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(sourceHtml);
            return GetInnerText(doc.DocumentNode, removeCommentTags, tagsToRemove);
        }

        public static HtmlNode GetNode(HtmlNode node,
            string element,
            string attribute,
            string valuePortion,
            bool tryNull = false)
        {
            if (tryNull)
            {
                try
                {
                    var _nodes = node.Descendants(element)
                        .Where(d => d.Attributes.Contains(attribute) &&
                                    d.Attributes[attribute].Value.Contains(valuePortion));

                    if (_nodes != null && _nodes.Count() > 0)
                    {
                        var _node = _nodes.ToArray()[0]; //.SingleOrDefault();
                        return _node;
                    }
                }
                catch
                {
                }
            }
            else
            {
                var _nodes = node.Descendants(element)
                    .Where(d => d.Attributes.Contains(attribute) &&
                                d.Attributes[attribute].Value.Contains(valuePortion));

                if (_nodes != null && _nodes.Count() > 0)
                {
                    var _node = _nodes.ToArray()[0]; //.SingleOrDefault();
                    return _node;
                }
            }

            return null;
        }

        public static HtmlNode GetNodeByInnerHtml(HtmlNode node,
            string element,
            //string innerHtml,
            string valuePartial)
        {
            var targetNode = node.Descendants(element).FirstOrDefault(d => d.InnerHtml.Contains(valuePartial));
            return targetNode;
        }

        public static HtmlNode GetNodeByAttribute(HtmlNode node,
            string element,
            string attribute,
            string valuePartial,
            bool tryNull = false)
        {
            var nodes = node.Descendants(element)
                .Where(d => d.Attributes.Contains(attribute) && d.Attributes[attribute].Value.Contains(valuePartial));

            if (tryNull)
            {
                try
                {
                    var htmlNodes = nodes as HtmlNode[] ?? nodes.ToArray();
                    if (!htmlNodes.Any()) return null;

                    var _node = htmlNodes.ToArray()[0]; //.SingleOrDefault();
                    return _node;

                }
                catch
                {
                    // ignored
                }
            }
            else
            {
                var htmlNodes = nodes as HtmlNode[] ?? nodes.ToArray();
                if (!htmlNodes.Any()) return null;

                var _node = htmlNodes.ToArray()[0]; //.SingleOrDefault();
                return _node;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="element"></param>
        /// <param name="attribute"></param>
        /// <param name="valuePartial"></param>
        /// <returns></returns>
        public static IEnumerable<HtmlNode> GetNodeCollection(HtmlNode node,
            string element,
            string attribute,
            string valuePartial)
        {
            var nodes = node.Descendants(element)
                .Where(d => d.Attributes.Contains(attribute) && d.Attributes[attribute].Value.Contains(valuePartial));
            return nodes;
        }

        public static IEnumerable<HtmlNode> GetNodeCollection(HtmlNode node, params string[] element)
        {
            var elements = node.Descendants();
            var nodes = elements.Where(p => element.Contains(p.Name));
            //var _nodes = node.Descendants(element).Where(d => d.Name.Contains(element));
            return nodes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="brute"></param>
        /// <returns></returns>
        public static bool IsURLValid(string url, bool brute = false)
        {
            if (!brute)
            {
                Uri uri = null;
                return Uri.TryCreate(url, UriKind.Absolute, out uri);
            }
            else
            {
                // using MyClient from linked post
                using (var client = new MyClient() {HeadOnly = true})
                {
                    try
                    {
                        var s1 = client.DownloadString(url);

                        // no error has occured so,
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal class MyClient : WebClient
        {
            public bool HeadOnly { get; set; }

            protected override WebRequest GetWebRequest(Uri address)
            {
                var req = base.GetWebRequest(address);
                if (req != null && (HeadOnly && req.Method == "GET"))
                {
                    req.Method = "HEAD";
                }

                return req;
            }
        }
    }
}