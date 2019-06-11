using System;
using System.Collections.Generic;
using System.Text;

namespace SMEAppHouse.Core.ScraperBox
{


    public class HttpOpsRules
    {
        public enum HttpMethodConsts
        {
            // ReSharper disable InconsistentNaming
            GET,
            POST,
            PUT,
            HEAD,
            TRACE,
            DELETE,
            SEARCH,
            CONNECT,
            PROPFIND,
            PROPPATCH,
            PATCH,
            MKCOL,
            COPY,
            MOVE,
            LOCK,
            UNLOCK,

            OPTIONS
            // ReSharper restore InconsistentNaming
        }

        public enum ContentTypeConsts
        {
            Xml,
            Json
        }
    }

    public class IPProxyRules
    {
        /// <summary>
        /// Expresses 3 levels of proxies according to their anonymity.
        /// </summary>
        public enum ProxyAnonymityLevelsEnum
        {
            /// <summary>
            /// Level 1 - Highly Anonymous Proxy: The web server can't detect whether you are using a proxy
            /// </summary>
            Elite,

            /// <summary>
            /// Level 2 - Anonymous Proxy: The web server can know you are using a proxy, but it can't know your real IP.
            /// </summary>
            Anonymous,

            /// <summary>
            /// Level 3 - Transparent Proxy: The web server can know you are using a proxy and it can also know your real IP.
            /// </summary>
            Transparent
        }

        public enum ProxySpeedsEnum
        {
            Slow, Medium, Fast
        }

        public enum ProxyConnectionSpeedsEnum
        {
            Slow, Medium, Fast
        }

        /// <summary>
        /// Secure websites whose url starts with https:// instead of http://
        /// (ex. https://www.paypal.com) use the encrypted (SSL/HTTPS) connections
        /// between its web server and the visitors. Some proxies only support ordinary
        /// http sites and can't surf https sites.
        /// Elite Proxy Switcher can test whether a proxy supports https sites.
        /// </summary>
        public enum ProxyProtocolsEnum
        {
            HTTP, HTTPS, SOCKS4_5
        }
    }

}
