using System;
using System.Net;
using SMEAppHouse.Core.CodeKits;

namespace SMEAppHouse.Core.ScraperBox.Models
{
    [Serializable]
    public class IPProxy
    {
        public Guid Id { get; private set; }
        public string ProviderId { get; set; }

        public string IPAddress { get; set; }
        public int PortNo { get; set; }
        public Rules.WorldCountriesEnum Country { get; set; }
        public IPProxyRules.ProxyAnonymityLevelsEnum AnonymityLevel { get; set; }
        public IPProxyRules.ProxyProtocolsEnum Protocol { get; set; }
        public DateTime LastChecked { get; set; }

        public int ResponseRate { get; set; }
        public int SpeedRate { get; set; }
        public TimeSpan SpeedTimeSpan => TimeSpan.FromMilliseconds(this.SpeedRate);
        public string ISP { get; set; }
        public string City { get; set; }

        public IPProxyRules.ProxySpeedsEnum Speed { get; set; }
        public IPProxyRules.ProxyConnectionSpeedsEnum ConnectionTime { get; set; }

        public Guid CheckerTokenId { get; set; }

        public CheckStatusEnum CheckStatus { get; set; }

        public Tuple<string, string> Credential { get; set; }

        public NetworkCredential ToNetworkCredential()
        {
            return this.Credential != null
                ? new NetworkCredential(this.Credential.Item1, this.Credential.Item2)
                : null;
        }

        public IPProxy()
        {
            Id = Guid.NewGuid();
        }

        public Tuple<string, string> AsTuple()
        {
            return new Tuple<string, string>(this.IPAddress, this.PortNo.ToString());
        }

        public override string ToString()
        {
            return $"{ProviderId} -> {IPAddress}:{PortNo}";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IWebProxy ToWebProxy()
        {
            return new WebProxy(IPAddress, PortNo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetLastValidationElapsedTime()
        {
            var lastValidationElapsedTime = DateTime.Now.Subtract(LastChecked);
            return lastValidationElapsedTime;
        }

        public enum CheckStatusEnum
        {
            NotChecked,
            Checking,
            Checked,
            CheckedInvalid,
        }
    }
}
