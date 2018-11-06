using System.Runtime.Serialization;

namespace SMEAppHouse.Core.GHClientLib.Model
{
    [DataContract]
    public class RouteOptimizationAddress
    {
        [DataMember(Name = "location_id")]
        public string LocationId { get; set; }
        [DataMember(Name = "lon")]
        public double Lon { get; set; }
        [DataMember(Name = "lat")]
        public double Lat { get; set; }
    }
}