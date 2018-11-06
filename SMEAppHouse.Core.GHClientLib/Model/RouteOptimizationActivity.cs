using System.Runtime.Serialization;

namespace SMEAppHouse.Core.GHClientLib.Model
{
    [DataContract]
    public class RouteOptimizationActivity
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }
        [DataMember(Name = "location_id")]
        public string LocationId { get; set; }
        [DataMember(Name = "end_time")]
        public string EndTime { get; set; }
        [DataMember(Name = "distance")]
        public string Distance { get; set; }
    }
}