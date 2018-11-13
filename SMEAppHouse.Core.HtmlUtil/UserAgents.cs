using ScrapySharp.Network;

namespace SMEAppHouse.Core.HtmlUtil
{
    public sealed class AuthenticationMethod
    {

        private readonly string _name;
        private readonly int _value;

        public static readonly AuthenticationMethod FORMS = new AuthenticationMethod(1, "FORMS");
        public static readonly AuthenticationMethod WINDOWSAUTHENTICATION = new AuthenticationMethod(2, "WINDOWS");
        public static readonly AuthenticationMethod SINGLESIGNON = new AuthenticationMethod(3, "SSN");

        private AuthenticationMethod(int value, string name)
        {
            this._name = name;
            this._value = value;
        }

        public override string ToString()
        {
            return _name;
        }

    }

    /// <summary>
    /// Type-safe-enum pattern.
    /// </summary>
    public sealed class UserAgents
    {
        private readonly string _name;
        private readonly FakeUserAgent _value;

        public static readonly UserAgents Mozilla22 = new UserAgents("Mozilla22", new FakeUserAgent("Mozilla 2.2", "Mozilla/5.0 (Windows; U; Windows NT 6.1; rv:2.2) Gecko/20110201")); // compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0
        public static readonly UserAgents FireFox36 = new UserAgents("FireFox36", new FakeUserAgent("FireFox 36.0", "Mozilla/5.0 (Windows NT 6.3; rv:36.0) Gecko/20100101 Firefox/36.0"));
        public static readonly UserAgents FireFox33 = new UserAgents("FireFox33", new FakeUserAgent("FireFox 33.0", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_10; rv:33.0) Gecko/20100101 Firefox/33.0"));
        public static readonly UserAgents Chrome41022280 = new UserAgents("Chrome41022280", new FakeUserAgent("Chrome 41.0.2228.0", "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2228.0 Safari/537.36"));
        public static readonly UserAgents InternetExplorer8 = new UserAgents("InternetExplorer8", new FakeUserAgent("Internet Explorer 8", "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; CMDTDF; .NET4.0C; .NET4.0E)"));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userAgent"></param>
        /// <returns></returns>
        public static FakeUserAgent GetFakeUserAgent(UserAgents userAgent)
        {
            if (userAgent == null) return Mozilla22._value;
            if (userAgent == Chrome41022280) return Chrome41022280._value;
            if (userAgent == FireFox33) return FireFox33._value;
            if (userAgent == FireFox36) return FireFox36._value;
            if (userAgent == InternetExplorer8) return InternetExplorer8._value;
            return Mozilla22._value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        private UserAgents(string name, FakeUserAgent value)
        {
            _name = name;
            _value = value;
        }

        public override string ToString()
        {
            return _name;
        }
    }
}
