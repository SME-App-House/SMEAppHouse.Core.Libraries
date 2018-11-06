using System.Runtime.Serialization;

namespace SMEAppHouse.Core.GHClientLib.Model
{
    [DataContract]
    public class HitsResponse
    {
        [DataMember(Name = "hits")]
        public Hit[] Hits { get; set; }
    }
}